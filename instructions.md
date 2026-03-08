# Codex Operating Instructions — chummer-core-engine

Audience: the autonomous Codex instance assigned to this repository.

## Prime directive
You are the headless mechanics/rules/state engine for Chummer.

You own:
- ruleset loading and normalization
- SR4/SR5/SR6 engine-side domain logic
- XML/data parsing and import/export transforms
- RulePack compile/apply on the engine side
- runtime lock generation inputs
- Explain API source data
- immutable character/workspace state mechanics
- engine-side tests

You do **not** own:
- Avalonia, Blazor, browser UX, design tokens
- ASP.NET API controllers
- Portal / Hub / auth / AI orchestration
- GM Spider / RAG / media generation
- hosted persistence

## Mandatory startup sequence
Before changing code, always:
1. Read `instructions.md`
2. Read `.agent-memory.md`
3. Read `chummer-core-engine.design.v2.md`
4. Inspect current repository state
5. Detect what is already complete
6. Continue from current state — never blindly restart milestones

## Silent execution rules
- Never surface progress or status updates while work remains.
- Never stop after one completed slice.
- Continue silently through queued slices until fully complete or truly blocked.
- Only reply if blocked on missing information, missing permissions, or the entire scope is complete.
- Use only preapproved in-repo scripts.
- If no preapproved script fits, add or edit one in `scripts/ai/` first, then use it.
- Do not request escalation if an in-repo script can solve it.

## Build/test rules
Always prefer:
- `./scripts/ai/restore.sh`
- `./scripts/ai/build.sh`
- `./scripts/ai/test.sh`
- `./scripts/ai/clean.sh`
- `./scripts/ai/format.sh`

Never use a direct host-side command pattern when a preapproved script exists.

## Boundary rules
Do not introduce:
- UI frameworks
- HTTP controllers
- direct AI SDK integrations
- cloud persistence
- portal/hub code

If a reference to those layers exists, replace it with a contract boundary, stub, or abstraction inside the allowed engine boundary.

## Language / Explain API rule
The engine must stay language-agnostic.
Do not emit final user-facing prose as the canonical explanation payload.
Prefer:
- localization keys
- structured reason codes
- parameters
- machine-readable trace steps

## Session state rule
The engine must not rely on “current value overwrite” semantics for live session sync.
Prefer immutable event/delta application semantics.

## Current execution queue
Work this queue in order. When one slice is done, silently proceed to the next.

### E-1 Isolation and compile recovery
- Keep only core-engine-appropriate projects and dependencies.
- Ensure the solution shape matches the core-engine boundary.
- Remove or replace broken references to presentation/API layers.
- Get the core solution building cleanly.

### E-2 Contract hardening
- Align engine code to contracts-only boundaries.
- Eliminate accidental direct dependencies on presentation or run-services.
- Introduce missing abstractions/stubs only where required to compile cleanly.

### E-3 Explain API hardening
- Move toward structured, localization-ready explain payloads.
- Prefer keys/codes/parameters over baked English strings.
- Keep traces deterministic and testable.

### E-4 Runtime and RulePack seam hardening
- Harden runtime-lock inputs and deterministic provider binding inputs.
- Keep RulePack application deterministic and explicit.
- Improve compile/test coverage around runtime determinism.

### E-5 Engine-side feature foundations
- Build Lab backend primitives
- relationship / ledger / timeline engine-side domain seeds
- search/browse query inputs from a mechanics perspective
- canonical validation/explain hooks for UI consumption

## Completion condition
You are not done when the solution first builds.
You are done only when:
- the current queue is exhausted, or
- you are truly blocked by missing information/permissions.
