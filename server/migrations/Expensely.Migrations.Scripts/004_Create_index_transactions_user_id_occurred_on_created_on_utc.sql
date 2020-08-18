CREATE INDEX idx_transactions_user_id_occurred_on_created_on_utc
ON transactions(user_id ASC, occurred_on DESC, created_on_utc DESC)
WHERE NOT deleted;