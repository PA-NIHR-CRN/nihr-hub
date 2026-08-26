# NIHR Hub

The NIHR Hub is a centrally-provided portal giving NIHR staff single-point access to approved cloud applications and internal content pages.

## Language

### Core concepts

**Application:**
An externally-linked tool surfaced as an icon on the Hub homepage (e.g. Gmail, Gemini). Registered in `HubApplicationSettings` config and rendered in the All Applications / Favourites grids.
_Avoid_: App, service, tool, link

**Hub Page:**
An internal content page within the Hub (e.g. Application Overview, Policies). Distinct from an Application — a Hub Page is not an externally-linked icon but a route served by the Hub itself.
_Avoid_: App, application (when referring to internal pages)

**Application Overview:**
The Hub Page at `/apps-overview` that documents all Applications and their usage guidance. Accessible via the service navigation.
_Avoid_: Help page, apps overview

**Policies:**
The Hub Page at `/policies` hosting IT policy content. Accessible via the service navigation alongside Application Overview.
_Avoid_: IT Policies page, policy page

**Default Applications:**
The set of Applications shown in a user's Favourites before they have customised them. Currently: Support, Directory, Policies (formerly NIHR Info for Staff).
_Avoid_: Default apps, default favourites

**Favourites:**
The personalised subset of Applications a user has chosen to pin to the top of the homepage. Stored per-user in DynamoDB.
_Avoid_: Pinned apps, saved apps

### Content and configuration

**Content:**
Human-authored copy surfaced to users (e.g. policy entry descriptions, banner message text). Delivered via `IContentProvider` and expected to eventually be CMS-managed.
_Avoid_: Config (when referring to authored text)

**Configuration:**
System or environment settings that control behaviour (e.g. banner enabled/disabled toggle, application URLs). Stored in `appsettings.json` (committed template) and `appsettings.user.json` (local overrides, not committed).
_Avoid_: Content (when referring to behaviour toggles)

**Banner:**
A notification component rendered at the top of the homepage when enabled, used by the IS Function to broadcast important updates. Enable/disable state is Configuration; the message text is Content.

### Icons

**Icon:**
The image asset representing an Application on the homepage grid. Stored in `wwwroot/images/app-icons/`. New standard format is SVG where available, otherwise PNG at 2x/96dp.
_Avoid_: Logo, image, thumbnail
