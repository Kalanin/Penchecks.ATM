const ACTIONS = ['Deposit', 'Withdraw', 'Transfer']

function ActionButtons({ selectedAction, onSelectAction }) {
  return (
    <div className="actions">
      {ACTIONS.map((action) => (
        <button
          key={action}
          type="button"
          className={action === selectedAction ? 'action selected' : 'action'}
          aria-pressed={action === selectedAction}
          onClick={() => onSelectAction(action)}
        >
          {action}
        </button>
      ))}
    </div>
  )
}

export default ActionButtons
