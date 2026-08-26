# ADR-0001: Use IContentProvider abstraction for all CMS-bound content

**Status:** Accepted
**Date:** 2026-08-26

## Context

Two Sprint 6 features (ISFUNC-783 Policies page, ISFUNC-827 Banner) require content (policy entries, banner message) that the IS Function must be able to update without requiring a developer and a code deployment. A full CMS integration is not feasible within the sprint window.

Chris O'Neill confirmed on 25 Aug 2026: hard-coding is acceptable for the sprint, but a seam must be left for future CMS integration.

## Decision

All content that is expected to eventually be CMS-managed is delivered through `IContentProvider` from the NIHR SDK (`NIHR.Infrastructure.Interfaces`). A `StaticContentProvider` implementation is registered for the initial release. The interface is registered in DI so a CMS-backed implementation can be swapped in later without changing views or controllers.

**Content** (delivered via `IContentProvider`): policy entry titles, descriptions and URLs; banner message text.

**Configuration** (delivered via `IConfiguration`/`appsettings`): banner enabled/disabled toggle, application URLs, environment-specific settings. Configuration is not routed through `IContentProvider`.

**Interim pragmatism:** Until a CMS is introduced, the `StaticContentProvider` reads content values from `appsettings` (via `IOptions<PoliciesSettings>` and `IOptions<BannerSettings>`) rather than hard-coding them. This means content is temporarily stored in config rather than a content store, but it remains behind the `IContentProvider` seam so the CMS swap remains a single-registration change. All content keys (`PolicyName`, `PolicyDescription`, `PolicyUrl`, `Banner:Message`) are present in `appsettings.json` as a committed template; actual values are set in `appsettings.user.json` locally and via environment-specific config in deployed environments.

Reference implementation in the NIHR SDK:
- Interface: https://github.com/PA-NIHR-CRN/nihr-sdk/blob/main/NIHR.Infrastructure/Interfaces/IContentProvider.cs
- Tag helper: https://github.com/PA-NIHR-CRN/nihr-sdk/blob/cfcf0eb021f24ccc825ab7076d3076e862ad39a0/NIHR.GovUk.AspNetCore.Mvc/TagHelpers/WithContentTagHelper.cs

## Alternatives considered

**Hard-code directly in views/controllers with no abstraction** — rejected because it makes the future CMS migration a bigger diff and leaves no obvious seam for environment-specific content.

**Introduce full CMS now** — rejected due to sprint timeline and the CMS strategy discussion still being in progress.

## Consequences

- All content surfaces use `IContentProvider`, creating a consistent pattern across the codebase.
- The static implementation must be environment-aware at the DI registration level so test and production can receive different content when needed.
- `appsettings.json` is the committed template (non-sensitive keys/structure). `appsettings.user.json` is used for local development overrides and is not committed to source control.
