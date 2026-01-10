INSERT INTO "AspNetUsers" (
    "Id",
    "DisplayName",
    "EmailConfirmed",
    "PhoneNumberConfirmed",
    "TwoFactorEnabled",
    "LockoutEnabled",
    "AccessFailedCount",
    "Location_Longitude",
    "Location_Latitude",
    "Location_Radius")
SELECT
    '019b861b-e4d3-7a68-a5ff-eba3c175fa6c',
    format('Static Test User'),
    1,
    1,
    0,
    0,
    0,
    53.57074108769329,
    9.984302847504159,
    5_000