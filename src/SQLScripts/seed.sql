
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
    gen_random_uuid(),
    format('user_%s_%s', gs, substr(md5(random()::text), 1, 6)),
    random(0, 1)::boolean,
    random(0, 1)::boolean,
    random(0, 1)::boolean,
    random(0, 1)::boolean,
    random(0, 4),
    random(-180, 180),
    random(-90, 90),
    random(1, 10)
FROM generate_series(1, 1_000) as gs;


INSERT INTO "Magic"."Cards"
("Name",
 "ScryfallUrl",
 "ScryfallId")
SELECT
    format('cardname_%s_%s', gs, substr(md5(random()::text), 1, 6)),
    format('https://random.test/%s', gs, substr(md5(random()::text), 1, 6)),
    gen_random_uuid()
FROM generate_series(1, 1_000) as gs;


INSERT INTO "Magic"."CollectionCards" ("UserId", "Condition", "Quantity", "CardId")
SELECT
    U."Id",
    random(0, 5),
    random(1, 20),
    c."Id"
FROM "AspNetUsers" as U
         FULL JOIN(SELECT "Magic"."Cards"."Id" FROM "Magic"."Cards") as C on 1=1;


INSERT INTO "Magic"."WantLists" ("UserId", "Name", "Accessibility")
SELECT
    "Id",
    format('wantlist_%s', substr(md5("UserName"), 1, 6)),
    random(0, 1)
FROM "AspNetUsers";

INSERT INTO "Magic"."WantedCards" ("MinimumCondition", "Quantity", "WantListId", "CardId")
SELECT
    random(0, 3),
    random(1, 5),
    W."Id",
    C."Id"
FROM "Magic"."WantLists" as W
         FULL JOIN "Magic"."Cards" as C on 1=1;