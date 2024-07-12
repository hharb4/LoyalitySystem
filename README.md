Projects in the Solution
  1- LoyalitySystem.Host
  2- LoyalitySystem.Application
  3- LoyalitySystem.Domain
  4- LoyalitySystem.Infrastructure
  5 -LoyalitySystem.Domain.Shared
  6- LoyalitySystem.Contracts
  7- LoyalitySystem.Tests

Keycloak
  Description: Keycloak is used for authentication and authorization. It is containerized and runs on localhost:8080.
  Default Credentials:
    Username: admin
    Password: admin
Configuration:
  Realm: master (used for simplicity - it should not be used for production purpose)
  Client ID: loyalty-system-client
Setup Instructions:
  Update the issuer signing key and client secret in the Keycloak configuration.
  Ensure that the client has the "direct access grants" enabled to allow token generation using the client ID and secret.


Notes:
  When a user earns a point in the system, it is created using the system user and not the user's account. This ensures that the authorization is based on a single user. In the system, there are three seeded users with the following IDs:

  5eaaa701-b148-4b6b-8a75-477036694078
  fddb12e2-3a9a-47e5-96dc-b82b11df87c9
  7faf0979-8548-4d81-82e8-303c5d2f040e
