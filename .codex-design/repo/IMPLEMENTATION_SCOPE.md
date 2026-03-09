
# Core Implementation Scope

`chummer-core-engine` owns deterministic mechanics, reducer-safe session mutation, runtime bundles, Explain traces, and the canonical engine contract plane.

Must not own:
- UI rendering or shell chrome
- hosted-service workflows
- provider routing
- play/mobile client implementation

Current purification focus:
- remove `Chummer.Presentation.Contracts` and `Chummer.RunServices.Contracts` source leaks
- quarantine legacy tooling out of the active engine solution
- keep `Chummer.Engine.Contracts` as the only canonical engine/shared DTO source
