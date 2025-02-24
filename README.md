# NIHR Hub - TEST

## Overview
The **NIHR Hub** is a .NET MVC application providing a landing page for NIHR colleagues to access available productivity applications. It serves as a central point of navigation and information, ensuring users can efficiently find and utilize the tools they need.

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Rider](https://www.jetbrains.com/rider/)
- [Git](https://git-scm.com/)
- Access to AWS Secrets Manager (for deployed environments)

### Local Development Setup
1. Clone the repository:
   ```sh
   git clone https://github.com/PA-NIHR-CRN/nihr-hub.git
   cd nihr-hub
   ```

2. Restore dependencies:
   ```sh
   dotnet restore
   ```

3. Create a local configuration file:
   - Copy `appsettings.json` and rename it as `appsettings.user.json`.
   - Modify `appsettings.user.json` with your local development settings.

4. Run the application:
   ```sh
   dotnet run
   ```

5. Open the application in your browser:
   ```
   http://localhost:5000
   ```

### Environment Configuration
- In local development, configuration is managed via `appsettings.user.json`.
- In deployed environments, secrets (such as database connections and API keys) are retrieved from AWS Secrets Manager.

## Important Notes
### Acceptable Use Policy (AUP) Version Management
The version of the **Acceptable Use Policy (AUP)** in the application configuration **must** be kept consistent with the AUP version published in Google Docs. Ensure that any changes to the published AUP are reflected in the application’s settings.

## Deployment
Deployment is handled using AWS infrastructure. Ensure that the appropriate AWS credentials and IAM permissions are configured for secret retrieval and application hosting.
