import { useCallback, useEffect, useState } from 'react'
import AccountList from './components/AccountList'
import ActionButtons from './components/ActionButtons'
import AmountForm from './components/AmountForm'
import TransferForm from './components/TransferForm'
import TransactionHistory from './components/TransactionHistory'
import { deposit, getAccounts, getHistory, transfer, withdraw } from './api/atmApi'
import './App.css'

function App() {
  const [accounts, setAccounts] = useState([])
  const [history, setHistory] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [selectedAction, setSelectedAction] = useState(null)

  const refresh = useCallback(async (signal) => {
    try {
      const [accountData, historyData] = await Promise.all([
        getAccounts(signal),
        getHistory(signal),
      ])

      setAccounts(accountData)
      setHistory(historyData)
      setError(null)
    } catch (err) {
      if (err.name !== 'AbortError') {
        setError('Could not load data. Is the backend running?')
      }
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    const controller = new AbortController()

    refresh(controller.signal)

    return () => controller.abort()
  }, [refresh])

  async function handleTransaction(accountName, amount) {
    const submit = selectedAction === 'Deposit' ? deposit : withdraw

    await submit(accountName, amount)
    await refresh()
  }

  async function handleTransfer(fromAccount, toAccount, amount) {
    await transfer(fromAccount, toAccount, amount)
    await refresh()
  }

  return (
    <main className="app">
      <h1>ATM</h1>

      <section className="panel">
        <h2>Accounts</h2>
        <AccountList accounts={accounts} loading={loading} error={error} />
      </section>

      <section className="panel">
        <h2>Actions</h2>
        <ActionButtons
          selectedAction={selectedAction}
          onSelectAction={setSelectedAction}
        />

        {(selectedAction === 'Deposit' || selectedAction === 'Withdraw') && (
          <AmountForm
            key={selectedAction}
            action={selectedAction}
            accounts={accounts}
            onSubmit={handleTransaction}
          />
        )}

        {selectedAction === 'Transfer' && (
          <TransferForm accounts={accounts} onSubmit={handleTransfer} />
        )}
      </section>

      <section className="panel">
        <h2>Transaction History</h2>
        <TransactionHistory
          history={history}
          loading={loading}
          error={error}
        />
      </section>
    </main>
  )
}

export default App
