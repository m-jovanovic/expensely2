CREATE TABLE users (
	id UUID CONSTRAINT "PK_users_id" PRIMARY KEY,
	first_name VARCHAR(100) NOT NULL,
	last_name VARCHAR(100) NOT NULL,
	email VARCHAR(256) NOT NULL,
	password_hash TEXT NOT NULL,
	created_on_utc TIMESTAMP NOT NULL,
	modified_on_utc TIMESTAMP NULL,
	deleted BOOLEAN NOT NULL DEFAULT FALSE,
	deleted_on_utc TIMESTAMP NULL
)