# Bank System Documentation

This is a simple console-based banking system implemented in C#. It allows users to create accounts, log in, and perform various banking operations such as deposit, withdrawal, fund transfer, balance checking, and password change.

## Table of Contents
- [Introduction](#introduction)
- [Functionalities](#functionalities)
- [Usage](#usage)
- [Screenshots](#screenshots)
- [License](#license)

## Introduction

The Bank System is a console application designed to simulate basic banking operations. It provides a user-friendly interface for account creation, login, and various banking functionalities.

## Functionalities

### 1. Create Account
   - Users can create a new account by providing their personal information.
   - The account details are stored in a file named `<phone_number>.dat`.

### 2. Log In
   - Users can log in with their phone number and password.
   - Account information is read from the corresponding data file.
   - After successful login, users can perform various banking operations.

### 3. Deposit
   - Users can deposit a specified amount into their account.
   - The account balance is updated, and the transaction is recorded in the data file.

### 4. Withdraw
   - Users can withdraw a specified amount from their account, provided sufficient balance.
   - The account balance is updated, and the transaction is recorded in the data file.

### 5. Transfer Funds
   - Users can transfer funds to another account.
   - The recipient's account is updated, and both transactions are recorded in the respective data files.

### 6. Check Balance
   - Users can check their account balance.

### 7. Change Password
   - Users can change their account password after providing the current password.

### 8. Exit
   - Users can exit the banking system.

## Usage

1. Compile the program using a C# compiler.
2. Run the compiled executable.
3. Follow the on-screen prompts to navigate through the menu and perform banking operations.

## Screenshots

**1. Main Menu:**
![Main Menu](screenshots/main_menu.png)

**2. Create Account:**
![Create Account](screenshots/create_account.png)

**3. Log In:**
![Log In](screenshots/login.png)

**4. User Menu:**
![User Menu](screenshots/user_menu.png)

**5. Deposit:**
![Deposit](screenshots/deposit.png)

**6. Withdraw:**
![Withdraw](screenshots/withdraw.png)

**7. Transfer Funds:**
![Transfer Funds](screenshots/transfer_funds.png)

**8. Check Balance:**
![Check Balance](screenshots/check_balance.png)

**9. Change Password:**
![Change Password](screenshots/change_password.png)

## License

This project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute it as per the license terms.

## Acknowledgments

We extend our sincere appreciation to the developers (s1gm9,WacoderForever, karisacodes) and contributors (G-CoderX, Stephanie Makori, Voste254, Nyabs254, 123Origami) who have generously contributed to enhancing this bank system.

