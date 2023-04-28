# Security Policy

The nature of RetireSimple is that vulnerabilities in the software or mishandling of data is very much important to resolve to protect somewhat sensitive data. We have a part of the documentation dedicated to this (see [Threat Model]((https://github.com/RetireSimple/RetireSimple/wiki/Threat-Model))), but we also expect that some new issues get discovered along the way. This is especially difficult given this project is partially a focused as a learning experience for college students. So we have the following rules/guidelines in keeping RetireSimple secure:

- We will only support security for the ***latest version of the application***, and will not backport fixes to older versions.
- If a dependency used by RetireSimple has a vulnerability where the engine/application could be a vector for an exploit, we will investigate the impact of updating the dependency as part of a fix.
- If the code of the RetireSimple engine/application has a vulnerability, we will investigate the impact of the vulnerability and will fix it as soon as possible.

## Reporting a Vulnerability or Security Concern

If you want to report a vulnerability or security concern, feel free to open an issue and mark it with the ***security*** issue tag. We would recommend outlining the mechanism and avoid posting PoC exploits in the issue unless asked to do so. You are also welcome to suggest or open a PR with a fix. If you are unsure if the issue is a security concern, feel free to open an issue and we will triage it.

We will close or redirect any issues that are not related to security vulnerabilities or concerns of the RetireSimple engine/application (i.e. it is actually a .NET issue, a dependency issue, etc.).

We reserve the right to update this policy at any time.