# Loyalty System

## Projects in the Solution
1. LoyaltySystem.Host
2. LoyaltySystem.Application
3. LoyaltySystem.Domain
4. LoyaltySystem.Infrastructure
5. LoyaltySystem.Domain.Shared
6. LoyaltySystem.Contracts
7. LoyaltySystem.Tests

## Keycloak
**Description:** Keycloak is used for authentication and authorization. It is containerized and runs on `localhost:8080`.

**Default Credentials:**
- Username: admin
- Password: admin

**Configuration:**
- Realm: `master` (used for simplicity - it should not be used for production purposes)
- Client ID: `loyalty-system-client`

**Setup Instructions:**
- Update the issuer signing key and client secret in the Keycloak configuration.
- Ensure that the client has the "direct access grants" enabled to allow token generation using the client ID and secret.

## Notes
When a user earns a point in the system, it is created using the system user and not the user's account. This ensures that the authorization is based on a single user. In the system, there are three seeded users with the following IDs:
- `5eaaa701-b148-4b6b-8a75-477036694078`
- `fddb12e2-3a9a-47e5-96dc-b82b11df87c9`
- `7faf0979-8548-4d81-82e8-303c5d2f040e`

Sql data and redis cached data are stored in volumes.

  ## Enhancements
  1. Decoupling Users and Points logic.
  2. Add Repository Service.
  3. Add helper class library for extensions in case of Microservices
  4. Create new realm in keycloak and configure users and claim
  5. Add scopes for users
  6. Create helper class for caching with interface regardless of the caching stack used
  7. Create separate unit testing classes
