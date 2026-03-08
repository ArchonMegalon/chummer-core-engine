# Codex Instructions

Read these files before coding:
1. instructions.md
2. .agent-memory.md
3. AGENT_MEMORY.md
4. chummer-core-engine.design.v2.md
5. AGENTS.md if present

Global rules:
- Inspect the current repository state before changing anything.
- Never restart completed work blindly.
- Continue from the repo's actual current state.
- Never surface progress or status updates while work remains.
- Never stop after one slice; silently continue with the next queued slice.
- Only respond if blocked on missing information, missing permissions, explicitly told to stop, or the entire requested scope is fully complete.
- Use only preapproved in-repo scripts.
- If no preapproved script fits, add or edit one in scripts/ai first, then run that script.
- Prefer scripts/ai/restore.sh, build.sh, test.sh, clean.sh, format.sh over direct host-side commands.

Completion rule:
- A green build is not enough.
- Continue until the queued slices from the design document are exhausted or you are truly blocked.
