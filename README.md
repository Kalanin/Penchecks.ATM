### Penchecks Trust ATM Take Home Assignment 

This project repo contains the full code for both the front end and backend functionality used to complete the given ATM project as requested. The following features were asked for:

- A web-based ATM interface supporting: deposit funds, withdraw funds, and transfer between the two accounts
- Track account balances and transaction history
- Single-user (no authentication required)

The setup for this application is incredibly simple, and deliberately so. There is no external DB or other set up required other than having the requisite NPM / node.js and .NET Core 10 frameworks.

## How to run

#Frontend

On opening up the repo, you can navigate to the ATM.Web folder

```cd Frontend\ATM.Web```

Running `npm run dev` from the terminal will start the frontend. It will be listening at `http://localhost:5174/`

#Backend

You can open the solution project under Backend and run the application through Visual Studio.

Alternatively:

```cd Backend\ATM.Service```

Running `dotnet run` from the terminal will start the backend. It uses both `https://localhost:7241` and `http://localhost:5241`

By default a "Checking" and "Savings" account are initialized on startup with a starting value of $0.00.

## Core Features

All Backend APIs are through the `/ATM` path

- Account information is retrieved through `GET /ATM`
- Transaction History is retrieved through `GET /ATM/history`
- Deposits are handled through `POST /ATM/deposit`
```
{
    "AccountName": "Savings",
    "Amount": 200
}
```
- Withdraws are handled through `POST /ATM/withdraw`
```
{
    "AccountName": "Savings",
    "Amount": 200
}
```
- Transfer are handled through `POST /ATM/transfer`
```
{
    "AccountName": "Savings",
    "Amount": 200
}
```

- An endpoint not wired into the frontend is also available `POST /ATM/register`. This endpoint is so that you can alter the initial state of the users account. *It will overwrite the existing accounts and wipe out transaction history when done to simulate a new users account*
```
{
    "Accounts": [
        {
            "name": "Checking",
            "amount": 1000
        },
        {
            "name": "Savings",
            "amount": 0
        }
    ]
}
```

The Backend was written through .NET C# primarily, and is structured around a single controller (ATMController) which calls and utilizes ATMService to handle all of it's requests. It utilizes in-memory storage, and consideration was given to alternative options (including even mimicing a SQL storage through csv file creation), but was not pursued in the interest of simplicity. It has some basic unit tests which all pass and address the core exceptions and error handling that was added to each method. 

The Frontend was setup through React.js, which is a framework I am still in the process of learning and am not overly familiar with. This was done as a learning oppurtunity for myself and because React is closer to Vue.js than Angular from my knowledge of the frameworks, and I believe this would be easier than Angular long term. In the interest of time, the front end was constructed by setting up through Vite first (`npm create vite@latest`) and then utilized AI agentic coding when the backend was created to help wire endpoints and provide a usable UI. The code was still vetted by myself however, and I understand it at a decent enough level that I should be able to explain it sufficiently.


