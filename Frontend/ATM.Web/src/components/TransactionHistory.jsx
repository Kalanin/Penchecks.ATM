function formatTimestamp(timestamp) {
  const date = new Date(timestamp)

  if (Number.isNaN(date.getTime())) {
    return timestamp
  }

  return date.toLocaleString()
}

function TransactionHistory({ history, loading, error }) {
  if (loading) {
    return <p className="status">Loading history...</p>
  }

  if (error) {
    return <p className="status error">{error}</p>
  }

  if (history.length === 0) {
    return <p className="status">No transactions yet.</p>
  }

  return (
    <table className="history">
      <thead>
        <tr>
          <th>Timestamp</th>
          <th>Type</th>
          <th>Message</th>
        </tr>
      </thead>
      <tbody>
        {history.map((entry, index) => (
          <tr key={`${entry.timestamp}-${index}`}>
            <td className="history-timestamp">{formatTimestamp(entry.timestamp)}</td>
            <td>{entry.type}</td>
            <td>{entry.log}</td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}

export default TransactionHistory
