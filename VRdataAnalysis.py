"""
VR Object Sorting Study — Data Analysis Script
Run from the folder that contains your P01, P02, P03... participant folders.

Usage:
    python analyse_vr_data.py

Output:
    Prints two summary tables to the terminal.
    Saves results to vr_analysis_results.csv
"""

import os
import glob
import pandas as pd

# ── CONFIG ────────────────────────────────────────────────────────────────────
DATA_ROOT = "/Users/shaniabrown/Desktop/HP/Research Project/Data/files/cleaned"       # folder containing P01, P02  with clean data 
MATCHED_OBJECTS = {"apple", "mug", "plate", "wrench"}   # the 4 paired objects

CATEGORY_MAP = {         #objects with lowpoly and highpoly pairs. best possible comparison
    "apple":  "Food",
    "mug":    "Kitchen",
    "plate":  "Kitchen",
    "wrench": "Tools",
}
# ──────────────────────────────────────────────────────────────────────────────


def load_all_data(root):
    frames = []
    participant_dirs = sorted(
        d for d in os.listdir(root)
        if os.path.isdir(os.path.join(root, d))
        and d.upper().startswith("P")
        and d.upper() not in ("TEST", "DEMO")   #dont use test/demo data
    )

    if not participant_dirs:
        raise FileNotFoundError(
            f"No participant folders (P01, P02 …) found in: {os.path.abspath(root)}"
        )

    print(f"Found participant folders: {participant_dirs}\n")

    for pid in participant_dirs:
        csv_files = glob.glob(os.path.join(root, pid, "*.csv"))
        if not csv_files:
            print(f"  WARNING: No CSVs found for {pid} — skipping")
            continue
        for f in csv_files:
            df = pd.read_csv(f)
            df["ParticipantID"] = pid  # use folder name, not CSV contents
            df["source_file"] = os.path.basename(f)
            frames.append(df)

    return pd.concat(frames, ignore_index=True)


def clean(df):
    # Standardise column names and string whitespace
    df.columns = df.columns.str.strip()
    for col in df.select_dtypes("object").columns:
        df[col] = df[col].str.strip()

    # Keep Environment scene only
    df = df[df["Realism"].str.lower() != "warmup"].copy()

    # Exclude TEST / DEMO sessions
    df = df[~df["ParticipantID"].str.upper().isin(["TEST", "DEMO"])]

    # Normalise object names to lowercase for matching
    df["ObjectKey"] = df["ObjectName"].str.lower().str.replace(r"^(real|lowpoly)\s*", "", regex=True)

    # Keep only the 4 matched objects
    df = df[df["ObjectKey"].isin(MATCHED_OBJECTS)].copy()

    # Deduplicate multi-trigger rows: keep first placement per participant/file/object
    df = df.sort_values("SessionTime")
    df = df.drop_duplicates(subset=["ParticipantID", "source_file", "ObjectName"], keep="first")

    # Add category
    df["Category"] = df["ObjectKey"].map(CATEGORY_MAP)

    # Ensure Correct is boolean
    if df["Correct"].dtype == object:
        df["Correct"] = df["Correct"].str.upper().map({"TRUE": True, "FALSE": False})

    return df


def per_participant_condition_means(df):
    """Average across runs per participant before pooling across participants."""
    grouped = (
        df.groupby(["ParticipantID", "source_file", "Realism"])
        .agg(Accuracy=("Correct", "mean"), MeanTime=("ObjectTime", "mean"))
        .reset_index()
    )
    # Average the runs per participant
    per_p = (
        grouped.groupby(["ParticipantID", "Realism"])
        .agg(Accuracy=("Accuracy", "mean"), MeanTime=("MeanTime", "mean"))
        .reset_index()
    )
    return per_p


def per_participant_category_means(df):
    grouped = (
        df.groupby(["ParticipantID", "source_file", "Realism", "Category"])
        .agg(Accuracy=("Correct", "mean"), MeanTime=("ObjectTime", "mean"))
        .reset_index()
    )
    per_p = (
        grouped.groupby(["ParticipantID", "Realism", "Category"])
        .agg(Accuracy=("Accuracy", "mean"), MeanTime=("MeanTime", "mean"))
        .reset_index()
    )
    return per_p


def fmt_pct(x):
    return f"{x * 100:.1f}%"

def fmt_sec(x):
    return f"{x:.2f}s"


# ── MAIN ──────────────────────────────────────────────────────────────────────
raw = load_all_data(DATA_ROOT)
df  = clean(raw)

print(f"Rows after cleaning: {len(df)}")
print(f"Participants: {sorted(df['ParticipantID'].unique())}")
print(f"Realism conditions: {sorted(df['Realism'].unique())}\n")

# ── TABLE 1: Overall condition comparison ────────────────────────────────────
pp = per_participant_condition_means(df)
table1 = (
    pp.groupby("Realism")
    .agg(
        N_Participants=("ParticipantID", "count"),
        Mean_Accuracy=("Accuracy", "mean"),
        Mean_ObjectTime=("MeanTime", "mean"),
    )
    .reset_index()
)
table1["Mean_Accuracy"]   = table1["Mean_Accuracy"].map(fmt_pct)
table1["Mean_ObjectTime"] = table1["Mean_ObjectTime"].map(fmt_sec)

print("=" * 55)
print("TABLE 1 — Overall Condition Comparison")
print("=" * 55)
print(table1.to_string(index=False))
print()

# ── TABLE 2: Category breakdown ──────────────────────────────────────────────
ppc = per_participant_category_means(df)
table2 = (
    ppc.groupby(["Category", "Realism"])
    .agg(
        Mean_Accuracy=("Accuracy", "mean"),
        Mean_ObjectTime=("MeanTime", "mean"),
    )
    .reset_index()
)
table2["Mean_Accuracy"]   = table2["Mean_Accuracy"].map(fmt_pct)
table2["Mean_ObjectTime"] = table2["Mean_ObjectTime"].map(fmt_sec)

print("=" * 55)
print("TABLE 2 — By Category")
print("=" * 55)
print(table2.to_string(index=False))
print()

# ── Save results ─────────────────────────────────────────────────────────────
out_path = os.path.join(DATA_ROOT, "vr_analysis_results.csv")
combined = pd.concat(
    [table1.assign(Table="Overall"), table2.assign(Table="ByCategory")],
    ignore_index=True
)
combined.to_csv(out_path, index=False)
print(f"Results saved to: {os.path.abspath(out_path)}")