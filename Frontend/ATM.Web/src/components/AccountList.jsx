function formatCurrency(amount) {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  }).format(amount)
}

function AccountList({ accounts, loading, error }) {
  if (loading) {
    return <p className="status">Loading accounts...</p>
  }

  if (error) {
    return <p className="status error">{error}</p>
  }

  if (accounts.length === 0) {
    return <p className="status">No accounts registered.</p>
  }

  return (
    <ul className="account-list">
      {accounts.map((account) => (
        <li key={account.name} className="account">
          <span className="account-name">{account.name}</span>
          <span className="account-amount">{formatCurrency(account.amount)}</span>
        </li>
      ))}
    </ul>
  )
}

export default AccountList
