﻿CREATE INDEX idx_transactions_transaction_type ON transactions(transaction_type)
WHERE NOT deleted;