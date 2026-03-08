
# chummer-core-engine.design.v2.md

Version: v2.0  
Status: authoritative design for Codex Instance A

## 1. Mission

`chummer-core-engine` is the **authoritative mechanics, content, state, and explainability engine** for Chummer.

It owns:

- SR4 / SR5 / SR6 ruleset loading and evaluation
- immutable character and workspace state
- XML ingestion, normalization, upgrade, and export
- RulePack compile/resolve
- RuntimeLock and runtime fingerprint generation
- typed capability ABI for rule providers
- Lua sandbox execution
- Explain traces
- BuildLab simulation primitives
- deterministic event application for session state

It does **not** own:

- HTTP hosting
- portal or Hub workflows
- authentication, reviews, moderation, install analytics
- GM Director / Spider orchestration
- provider routing for 1min.AI / AI Magicx / BrowserAct / Mootion
- PDF, screenshot, or video generation jobs
- UI rendering

This repo is the place where **truth** is computed.

---

## 2. Product responsibilities

## 2.1 Base Chummer features owned here

The engine must provide the computational substrate for:

1. **Explain Everywhere**
   - all derived values
   - legality/eligibility failures
   - provider origin and pack origin
   - before/after diffs for runtime changes

2. **Build Lab**
   - evaluate concept variants
   - simulate 25 / 50 / 100 Karma progressions
   - detect trap choices or mutually counterproductive paths
   - emit structured build recommendations, not prose

3. **Runtime Inspector**
   - show active ruleset, RuleProfile, RulePacks, providers, and capability bindings
   - diff RuntimeLocks
   - validate compatibility

4. **Search-first browse semantics**
   - expose normalized metadata and disable-reason semantics
   - expose structured filter facts used by Presentation

5. **Character templates / BuildKits**
   - validate and apply template semantics
   - resolve template compatibility against RuntimeLock

6. **Session event application**
   - accept event deltas only
   - apply ordered `SessionOverlayEventDto` streams to derive canonical session state
   - no absolute state overwrite API

7. **Localization-safe explainability**
   - engine remains language-agnostic
   - explanations return localization keys and parameter payloads, never final English strings

## 2.2 Companion features this repo supports but does not own

The engine must expose structured outputs that can be used by `chummer.run-services` for:

- Chummer Coach
- Portrait Forge prompts
- Johnson's Briefcase dossier inputs
- Session Memory Engine summaries
- Shadowfeed consequence seeds
- NPC archetype metadata
- Route Cinema summary seeds

The engine emits **structured seeds**, not final marketing text or media.

---

## 3. Architecture principles

1. **Deterministic first**
   - same RuntimeLock + same character + same event stream = same result

2. **Localization-agnostic**
   - no final user-facing prose in engine outputs
   - use `ReasonKey`, `ExplanationKey`, and parameters

3. **No transport assumptions**
   - engine code must not care whether it runs in process, behind gRPC, HTTP, or inside a worker

4. **Provider outputs are inspectable**
   - every rule contribution has provider ID, pack ID, capability, and effect delta

5. **State is immutable**
   - characters and workspace snapshots are immutable
   - session state is rebuilt from ordered deltas

6. **No hidden AI**
   - no LLM providers inside the engine
   - no "smart" guesses in legal or mechanical results

---

## 4. Domain model

## 4.1 Core aggregates

- `CharacterDocument`
- `CharacterVersion`
- `Workspace`
- `RuntimeLock`
- `ResolvedRuntime`
- `RulePackManifest`
- `BuildKit`
- `ExplainTrace`
- `SessionOverlayProjection`

## 4.2 Session state model

The engine must never accept `CurrentEdge = 2` style mutation commands.

It only accepts immutable deltas such as:

- `tracker.increment`
- `tracker.decrement`
- `effect.applied`
- `effect.removed`
- `ammo.spent`
- `ammo.reloaded`
- `note.added`
- `pin.changed`

Then it derives the state projection by replaying the ordered stream.

This is required because Presentation is offline-capable and Run Services may generate valid concurrent events. A naive last-write-wins model is forbidden.

## 4.3 Explain trace model

Replace freeform human-readable strings with:

- `TargetKey`
- `FinalValue`
- `SummaryKey`
- `SummaryParameters`
- `TraceStepDto[]`

Each trace step must carry:

- provider ID
- source pack ID
- localization key
- localization parameters
- modifier applied
- category
- capability
- confidence / certainty flag if relevant

Presentation is responsible for localization and final rendering.

---

## 5. RulePack and RuntimeLock responsibilities

This repo owns:

- parse and validate RulePack manifests
- resolve dependency graph
- compose content bundles
- compile provider bindings
- emit deterministic RuntimeLock
- generate content-addressed runtime fingerprints

The fingerprint must be derived from:

- ruleset ID
- engine ABI version
- resolved content bundles
- pack IDs and versions
- asset checksums
- provider binding map
- capability ABI versions

A profile name or pack display title must never affect the fingerprint.

---

## 6. Lua and typed capability ABI

## 6.1 Goal

Move hardcoded rules out of C# into a typed rules runtime without sacrificing determinism or explainability.

## 6.2 Capability model

The engine must replace dictionary-shaped opaque rule payloads with typed capability contracts, for example:

- `derive.attribute-limit`
- `derive.initiative`
- `validate.choice`
- `validate.character`
- `availability.item`
- `price.item`
- `filter.choices`
- `effect.apply`
- `buildlab.recommendation`
- `session.quickaction`

Each capability must define:

- input DTO
- output DTO
- deterministic semantics
- explainability support
- gas budget / instruction budget
- session-safe flag
- localization key set

## 6.3 Sandbox requirements

The Lua runtime must provide:

- deterministic gas metering
- no network access
- no filesystem access
- no process spawning
- memory ceiling
- timeout ceiling
- provider trace reporting
- reproducible failure output

---

## 7. Feature design that belongs here

## 7.1 Explain Everywhere backend

Required outputs:

- final numeric result
- value history
- source provider bindings
- disabled reasons
- compatibility warnings
- before/after diffs when RuntimeLock changes

## 7.2 Build Lab backend

The engine must expose:

- `GenerateBuildVariants`
- `ScoreBuildVariant`
- `ProjectKarmaSpend`
- `DetectTrapChoices`
- `DetectRoleOverlap`
- `SuggestCorePackages`

These methods return ranked structured results, not prose.

## 7.3 Relationship and heat computation

The engine does not own the campaign graph, but it should own reusable computation primitives for:

- heat thresholds
- notoriety/public awareness transformations
- favor debt arithmetic
- downtime progression
- addiction and healing schedules
- faction-response seeds

This allows presentation and run-services to remain thin.

## 7.4 Character aesthetic digest

To support Portrait Forge without embedding AI logic here, expose a structured digest:

- metatype
- role tags
- build tags
- visible ware / magical style hints
- outfit archetype hints
- faction/corp style hints
- mood tags
- recent trauma / scars / condition hints
- background motifs

This is a semantic seed, not a prompt.

## 7.5 Dossier/export semantic seed

Expose neutral structured exports used by media systems:

- `CharacterDossierSeed`
- `NpcDossierSeed`
- `RunSummarySeed`
- `BuildIdeaSeed`
- `ShadowfeedSeed`

---

## 8. Localization contract

The engine must never bake English text into cross-boundary DTOs.

Every user-facing explanation must be represented as:

- key
- parameter list
- optional fallback key
- severity
- source pack / provider

All localized strings come from Presentation or Run Services using the active language pack.

This is mandatory for Pegasus and other localized content.

---

## 9. Testing requirements

This repo must own:

- deterministic golden tests
- legacy import/export tests
- shadow testing harness against legacy outputs
- RulePack compile tests
- RuntimeLock stability tests
- explain-trace snapshot tests
- localization-key presence tests
- session event replay tests
- BuildLab ranking regression tests

No feature is complete without golden fixtures.

---

## 10. Forbidden shortcuts

Never:

- emit final translated prose
- accept absolute session tracker values from other repos
- hide provider origins
- make HTTP calls to Hub or AI providers
- store user/campaign state in external DBs
- read browser-only or platform UI APIs

---

## 11. Repo boundaries and dependencies

Allowed:
- primitives/contracts packages
- XML and serialization libs
- Lua runtime libs
- benchmarks
- test frameworks

Forbidden:
- ASP.NET Core host packages
- EF Core
- SignalR
- Telegram/WhatsApp SDKs
- AI/LLM SDKs
- PDF or video generators

---

## 12. Relevant LTD integrations

This repo must not integrate directly with any owned LTDs.

It only emits structured outputs that other repos may feed into:
- 1min.AI
- AI Magicx
- Prompting Systems
- MarkupGo
- PeekShot
- Mootion
- AvoMap
- Documentation.AI

---

## 13. First milestones for this repo

### Milestone A1 — Localization-safe Explain API
Deliver:
- key/parameter-based explain DTOs
- localization-key tests
Exit:
- no engine DTO returns final prose strings

### Milestone A2 — Typed capability ABI
Deliver:
- typed capability contracts
- bridge adapters for old providers
Exit:
- no new rule providers use dictionary payloads

### Milestone A3 — Event-only session projection
Deliver:
- delta-only session application
- replay tests
Exit:
- no API accepts absolute tracker overwrite

### Milestone A4 — Build Lab engine
Deliver:
- structured variant and progression outputs
Exit:
- presentation can render Build Lab without adding any rules

### Milestone A5 — Aesthetic and dossier seeds
Deliver:
- semantic seeds for portrait/dossier generation
Exit:
- run-services can build media prompts without parsing character internals

---

## 14. What Codex Instance A should do first

1. replace human-readable explain strings with localization keys
2. formalize typed capability contracts
3. lock RuntimeLock fingerprint tests
4. add delta-only session replay
5. add Build Lab simulation DTOs
6. add semantic seeds for media-generating repos
