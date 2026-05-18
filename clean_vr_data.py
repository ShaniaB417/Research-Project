"""
VR Data Cleaner — fixes the Realism column across all participant CSVs.

- Rows where ObjectName is "Sphere" or "cube" → Realism = "Warmup"
- Rows starting with "Real " → Realism = "Realistic"
- Rows starting with "Lowpoly " → Realism = "LowPoly"

Originals are untouched. Cleaned files are saved to Data/files/cleaned/
with the same filenames.

Usage:
    python3 clean_vr_data.py
"""

import os
import glob
import pandas as pd

DATA_ROOT = "Data/files"
OUTPUT_ROOT = "Data/files/cleaned"

WARMUP_OBJECTS = {"Sphere", "cube"} #exclude warmup objects 

os.makedirs(OUTPUT_ROOT, exist_ok=True)

participant_dirs = sorted(
    d for d in os.listdir(DATA_ROOT)
    if os.path.isdir(os.path.join(DATA_ROOT, d))
    and d.upper().startswith("P")
    and d.upper() not in ("TEST", "DEMO")
)

print(f"Found participants: {participant_dirs}\n")

for pid in participant_dirs:
    csv_files = glob.glob(os.path.join(DATA_ROOT, pid, "*.csv"))
    out_dir = os.path.join(OUTPUT_ROOT, pid)
    os.makedirs(out_dir, exist_ok=True)

    for f in csv_files:
        df = pd.read_csv(f)
        df.columns = df.columns.str.strip()
        df["ObjectName"] = df["ObjectName"].str.strip()

        # Fix Realism column
        def fix_realism(row):
            name = row["ObjectName"]
            if name in WARMUP_OBJECTS:
                return "Warmup"
            elif name.startswith("Real"):
                return "Realistic"
            elif name.lower().startswith("lowpoly"):
                return "LowPoly"
            else:
                return row["Realism"]  # leave anything unexpected as-is

        df["Realism"] = df.apply(fix_realism, axis=1)

        out_path = os.path.join(out_dir, os.path.basename(f))
        df.to_csv(out_path, index=False)
        print(f"  Cleaned: {pid}/{os.path.basename(f)}")

print(f"\nDone. Cleaned files saved to: {os.path.abspath(OUTPUT_ROOT)}")