# BANK_SYSTEM
# Bank System Documentation

## Overview

The Bank System is a console-based application that provides basic banking functionalities such as account creation, login, deposit, withdrawal, fund transfer, balance checking, and password change. It is written in C#.

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Usage](#usage)
4. [Code Structure](#code-structure)
5. [Functionality Details](#functionality-details)
    - [1. Create Account](#1-create-account)
    - [2. Log In](#2-log-in)
    - [3. Deposit](#3-deposit)
    - [4. Withdraw](#4-withdraw)
    - [5. Transfer Funds](#5-transfer-funds)
    - [6. Check Balance](#6-check-balance)
    - [7. Change Password](#7-change-password)
6. [Conclusion](#conclusion)

## Introduction

The Bank System allows users to create accounts, log in, and perform various banking operations through a console interface. It uses file storage to persist user data, including account information and balances.

## Features

- **Account Management:** Users can create a new account by providing personal information.
- **Login:** Existing users can log in by entering their phone number and password.
- **Deposit:** Users can deposit funds into their accounts.
- **Withdraw:** Users can withdraw funds from their accounts, provided they have sufficient balance.
- **Transfer Funds:** Users can transfer funds to other accounts.
- **Check Balance:** Users can check their account balance.
- **Change Password:** Users can change their account password for security.

## Usage

1. Run the application.
2. Choose from the main menu options: create an account, log in, or exit.
3. Follow the prompts to perform desired operations.

## Code Structure

The code consists of a `BankSystem` class containing various static methods for different functionalities. The `Person` struct represents user information, and user data is stored in files named after their phone numbers.

## Functionality Details

### 1. Create Account

- Users can create an account by providing their first name, phone number, account number, password, and initial balance.
- Account data is stored in a file named after the user's phone number.

### 2. Log In

- Users can log in by entering their phone number and password.
- If successful, the user is presented with a menu to perform different operations.

### 3. Deposit

- Users can deposit funds into their accounts.
- The account balance is updated, and the changes are saved to the corresponding file.

### 4. Withdraw

- Users can withdraw funds from their accounts, provided they have sufficient balance.
- The account balance is updated, and the changes are saved to the corresponding file.

### 5. Transfer Funds

- Users can transfer funds to other accounts.
- The recipient's account is identified by the account number.
- Balances of both sender and recipient are updated, and changes are saved to the corresponding files.

### 6. Check Balance

- Users can check their account balance, which is displayed on the console.

### 7. Change Password

- Users can change their account password after entering the current password.
- The new password is updated, and changes are saved to the corresponding file.

## Conclusion

The Bank System provides basic banking operations through a console interface. Users can manage their accounts securely, perform transactions, and check their balances. The application uses file storage for data persistence.
