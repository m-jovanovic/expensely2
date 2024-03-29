﻿CREATE TABLE transactions (
	id UUID CONSTRAINT "PK_transactions_id" PRIMARY KEY,
	user_id UUID NOT NULL CONSTRAINT "FK_user_id" REFERENCES users(id),
	name VARCHAR(100) NOT NULL,
	amount NUMERIC(19,4) NOT NULL,
	currency INT NOT NULL,
	transaction_type INT NOT NULL,
	occurred_on DATE NOT NULL,
	created_on_utc TIMESTAMP NOT NULL,
	modified_on_utc TIMESTAMP NULL,
	deleted BOOLEAN NOT NULL DEFAULT FALSE,
	deleted_on_utc TIMESTAMP NULL
)