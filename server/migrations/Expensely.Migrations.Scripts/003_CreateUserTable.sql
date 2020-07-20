﻿CREATE TABLE "User" (
	"Id" UUID CONSTRAINT "PK_User_Id" PRIMARY KEY,
	"FirstName" VARCHAR(100) NOT NULL,
	"LastName" VARCHAR(100) NOT NULL,
	"Email" VARCHAR(256) NOT NULL,
	"PasswordHash" TEXT NOT NULL,
	"CreatedOnUtc" TIMESTAMP NOT NULL,
	"ModifiedOnUtc" TIMESTAMP NULL,
	"DeletedOnUtc" TIMESTAMP NULL,
	"Deleted" BOOLEAN NOT NULL DEFAULT FALSE
)