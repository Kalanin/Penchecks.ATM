const BASE_URL = '/api/ATM'

// The API may report failures as plain text, ProblemDetails, or { message }.
async function readErrorMessage(response) {
  try {
    const text = await response.text()

    if (!text) {
      return null
    }

    try {
      const body = JSON.parse(text)
      return body.detail ?? body.title ?? body.message ?? null
    } catch {
      return text
    }
  } catch {
    return null
  }
}

async function request(path, options) {
  const response = await fetch(`${BASE_URL}${path}`, options)

  if (!response.ok) {
    const message = await readErrorMessage(response)

    throw new Error(message || `Request failed with status ${response.status}`)
  }

  return response
}

export async function getAccounts(signal) {
  const response = await request('', { signal })
  return response.json()
}

export async function getHistory(signal) {
  const response = await request('/history', { signal })
  return response.json()
}

export async function deposit(accountName, amount) {
  await postTransaction('/deposit', { accountName, amount })
}

export async function withdraw(accountName, amount) {
  await postTransaction('/withdraw', { accountName, amount })
}

export async function transfer(fromAccount, toAccount, amount) {
  await postTransaction('/transfer', { fromAccount, toAccount, amount })
}

async function postTransaction(path, body) {
  await request(path, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  })
}
