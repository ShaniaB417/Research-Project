"""
VR Study — Qualtrics Survey Analysis
Calculates mean score per question and outputs a summary table.

Usage:
    python3 analyse_survey.py

Place this script in the same folder as your Qualtrics CSV export.
Update SURVEY_FILE below if your filename is different.
"""

import pandas as pd

SURVEY_FILE = "survey.csv"

QUESTIONS = {
    "Q1": "How mentally demanding was the sorting task?",
    "Q2": "How much did you have to concentrate?",
    "Q3": "How focused were you on the task and environment?",
    "Q4": "How real did the virtual objects feel?",
    "Q5": "How confident did you feel sorting the objects?",
    "Q6": "How natural did the objects feel to interact with?",
    "Q7": "How easy was it to work with the objects?",
}

# Qualtrics exports 3 header rows — skip first 2, use row 1 as header
df = pd.read_csv(SURVEY_FILE, header=0, skiprows=[1, 2])

# Extract just the question score columns (not the NPS_GROUP columns)
q_cols = ["Q1", "Q2", "Q3", "Q4", "Q5", "Q6", "Q7"]
scores = df[q_cols].apply(pd.to_numeric, errors="coerce")

# Assign participant IDs in order
scores.insert(0, "ParticipantID", [f"P0{i+1}" for i in range(len(scores))])

print("=" * 65)
print("RAW SCORES PER PARTICIPANT (0-10 scale)")
print("=" * 65)
print(scores.to_string(index=False))
print()

# Summary table
summary = pd.DataFrame({
    "Question": [QUESTIONS[q] for q in q_cols],
    "Mean Score": [scores[q].mean() for q in q_cols],
    "Min": [scores[q].min() for q in q_cols],
    "Max": [scores[q].max() for q in q_cols],
})
summary["Mean Score"] = summary["Mean Score"].map(lambda x: f"{x:.1f}")
summary["Min"] = summary["Min"].map(lambda x: f"{x:.0f}")
summary["Max"] = summary["Max"].map(lambda x: f"{x:.0f}")

print("=" * 65)
print("TABLE 3 — Survey Summary (n=4, scale 0–10)")
print("=" * 65)
print(summary.to_string(index=False))
print()

# Save
summary.to_csv("survey_results.csv", index=False)
print("Results saved to: survey_results.csv")