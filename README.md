# Baynetna

**Baynetna** is an anonymous feedback platform for reporting and discussing public service issues in Lebanon. Built with ASP.NET Core MVC, it prioritizes user privacy, accessibility, and civic engagement.

---

## 🌐 Features

- 🗳️ **Anonymous Posts** with unique thread IDs
- 👍 **Upvote/Downvote with Reason** required for each vote
- 🧾 **Token-Based Registration** with optional ID verification
- 🔐 **Simple Auth** using only username and password
- 🌍 **Multilingual Support** (English and Arabic bodies per post/comment)
- 🔁 **Moderator Tools** for managing and quarantining content
- 🔇 **Transparency**: Hidden but viewable deleted comments
- 🎤 **Voice Message Support** for posts and comments
- 🔎 **Search & Filtering** by ministry (wizara), date, likes, and comments
- 🧩 **Pagination** and sorting for posts and comments
- 🧠 **Accessibility**: 
  - Adjustable font size
  - Voice-to-text and AI narration (planned)
  - Color-blind friendly themes
  - Dark mode toggle

---

## 📊 Voting System

- Votes require a reason (like Stack Overflow)
- Votes are limited to one per user per post
- Super upvoted quarantined posts notify moderators

---

## 🛠️ Tech Stack

- Backend: **.NET Core MVC**
- Database: **SQL Server**
- Frontend: **Razor Views + Bootstrap** (Mobile-first design)
- Authentication: Custom (username + password only)
- Localization: English and Arabic
- File Storage: Voice message URLs (optional integration with blob storage)

---
