# Audit — core

- Generated: 2026-03-08T08:51:33+01:00
- Repo root: /docker/chummercomplete/chummer-core-engine
- Design doc expected: chummer-core-engine.design.v2.md

## Solutions
Chummer.CoreEngine.sln
Chummer.sln
Chummer/Chummer.sln
Translator/Translator.sln

## Projects
Chummer.Application/Chummer.Application.csproj
Chummer.Benchmarks/Chummer.Benchmarks.csproj
Chummer.Contracts/Chummer.Contracts.csproj
Chummer.Core/Chummer.Core.csproj
Chummer.Infrastructure.Browser/Chummer.Infrastructure.Browser.csproj
Chummer.Infrastructure/Chummer.Infrastructure.csproj
Chummer.Rulesets.Hosting/Chummer.Rulesets.Hosting.csproj
Chummer.Rulesets.Sr4/Chummer.Rulesets.Sr4.csproj
Chummer.Rulesets.Sr5/Chummer.Rulesets.Sr5.csproj
Chummer.Rulesets.Sr6/Chummer.Rulesets.Sr6.csproj
Chummer.Tests/Chummer.Tests.csproj
Chummer/Chummer.csproj
ChummerDataViewer/ChummerDataViewer.csproj
CrashHandler/CrashHandler.csproj
Plugins/ChummerHub.Client/ChummerHub.Client.csproj
Plugins/ChummerHub.Client/OidcClient/IdentityTokenValidator/IdentityTokenValidator.csproj
Plugins/ChummerHub.Client/OidcClient/OidcClient/OidcClient.csproj
Plugins/SamplePlugin/SamplePlugin.csproj
TextblockConverter/Textblock-Converter.csproj
Translator/Translator.csproj

## Top-level directories
.aider.tags.cache.v4
.git
.github
Chummer
Chummer.Application
Chummer.Benchmarks
Chummer.Contracts
Chummer.Core
Chummer.CoreEngine.Tests
Chummer.Infrastructure
Chummer.Infrastructure.Browser
Chummer.Rulesets.Hosting
Chummer.Rulesets.Sr4
Chummer.Rulesets.Sr5
Chummer.Rulesets.Sr6
Chummer.Tests
ChummerDataViewer
CrashHandler
Plugins
TextblockConverter
Translator
docs
git
scripts
settings

## Key instruction files
present: instructions.md
present: .agent-memory.md
present: AGENT_MEMORY.md
present: AGENTS.md
present: chummer-core-engine.design.v2.md

## Recommendation
Keep only engine/rules/contracts/tests. Remove or abstract any presentation/API leakage.
