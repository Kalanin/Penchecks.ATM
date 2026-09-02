import { useState } from 'react'

function TransferForm({ accounts, onSubmit }) {
  const [fromAccount, setFromAccount] = useState('')
  const [toAccount, setToAccount] = useState('')
  const [amount, setAmount] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState(null)

  function handleFromChange(event) {
    const value = event.target.value

    setFromAccount(value)

    if (value === toAccount) {
      setToAccount('')
    }
  }

  async function handleSubmit(event) {
    event.preventDefault()

    const parsedAmount = Number(amount)

    if (!fromAccount || !toAccount) {
      setError('Please select both accounts.')
      return
    }

    if (fromAccount === toAccount) {
      setError('Please select two different accounts.')
      return
    }

    if (!Number.isFinite(parsedAmount) || parsedAmount <= 0) {
      setError('Please enter an amount greater than zero.')
      return
    }

    setSubmitting(true)
    setError(null)

    try {
      await onSubmit(fromAccount, toAccount, parsedAmount)
      setAmount('')
    } catch (err) {
      setError(err.message || 'Transfer failed. Please try again.')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <form className="amount-form" onSubmit={handleSubmit}>
      <label htmlFor="from-account">From</label>
      <select
        id="from-account"
        value={fromAccount}
        onChange={handleFromChange}
      >
        <option value="">Select an account</option>
        {accounts.map((account) => (
          <option key={account.name} value={account.name}>
            {account.name}
          </option>
        ))}
      </select>

      <label htmlFor="to-account">To</label>
      <select
        id="to-account"
        value={toAccount}
        onChange={(event) => setToAccount(event.target.value)}
      >
        <option value="">Select an account</option>
        {accounts
          .filter((account) => account.name !== fromAccount)
          .map((account) => (
            <option key={account.name} value={account.name}>
              {account.name}
            </option>
          ))}
      </select>

      <label htmlFor="transfer-amount">Amount</label>
      <input
        id="transfer-amount"
        type="number"
        min="0.01"
        step="0.01"
        placeholder="0.00"
        value={amount}
        onChange={(event) => setAmount(event.target.value)}
      />

      {error && <p className="status error">{error}</p>}

      <button type="submit" className="submit" disabled={submitting}>
        {submitting ? 'Working...' : 'Transfer'}
      </button>
    </form>
  )
}

export default TransferForm
