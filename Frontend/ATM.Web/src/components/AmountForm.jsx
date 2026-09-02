import { useState } from 'react'

function AmountForm({ action, accounts, onSubmit }) {
  const [accountName, setAccountName] = useState('')
  const [amount, setAmount] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState(null)

  async function handleSubmit(event) {
    event.preventDefault()

    const parsedAmount = Number(amount)

    if (!accountName) {
      setError('Please select an account.')
      return
    }

    if (!Number.isFinite(parsedAmount) || parsedAmount <= 0) {
      setError('Please enter an amount greater than zero.')
      return
    }

    setSubmitting(true)
    setError(null)

    try {
      await onSubmit(accountName, parsedAmount)
      setAmount('')
    } catch (err) {
      setError(err.message || `${action} failed. Please try again.`)
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <form className="amount-form" onSubmit={handleSubmit}>
      <label htmlFor="account">Account</label>
      <select
        id="account"
        value={accountName}
        onChange={(event) => setAccountName(event.target.value)}
      >
        <option value="">Select an account</option>
        {accounts.map((account) => (
          <option key={account.name} value={account.name}>
            {account.name}
          </option>
        ))}
      </select>

      <label htmlFor="amount">Amount</label>
      <input
        id="amount"
        type="number"
        min="0.01"
        step="0.01"
        placeholder="0.00"
        value={amount}
        onChange={(event) => setAmount(event.target.value)}
      />

      {error && <p className="status error">{error}</p>}

      <button type="submit" className="submit" disabled={submitting}>
        {submitting ? 'Working...' : action}
      </button>
    </form>
  )
}

export default AmountForm
