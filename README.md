# Meeting Halls Reservation System

## Project Overview

The **Meeting Halls Reservation System** is a web-based application designed to manage the reservation of company meeting rooms. This system allows employees from different departments to book meeting rooms based on their availability and department needs. Additionally, it generates reports of the meetings that took place, including the results and outcomes of each meeting.

This system is built using **Web Forms** for the frontend and **SQL Server** for the backend database to store user data, meeting room reservations, and the generated reports.

## Features

- **User Authentication**: Employees must log in using their credentials to access the system.
- **Room Booking**: Allows users to select a meeting room, specify the date and time, and submit a reservation request.
- **Departmental Access**: Each department can manage their own meeting room reservations.
- **Report Generation**: After a meeting concludes, the system can generate a report with the results and outcomes of the meeting.
- **Admin Access**: Admins can view all reservations, edit or delete them, and manage user roles.

## Technologies Used

- **Frontend**: Web Forms (ASP.NET)
- **Backend**: SQL Server Database
- **Programming Languages**: C#, HTML, CSS, JavaScript
- **Libraries & Frameworks**: ASP.NET Web Forms, ADO.NET
- **Database**: SQL Server
- **Reporting**: SQL Reporting or custom reporting functionality for meeting results

## Installation

### Set Up the Database:

1. Open **SQL Server Management Studio (SSMS)**.
2. Create a new database and import the provided `.sql` scripts to set up the necessary tables.
3. Ensure that the **connection string** in the `web.config` file matches your local database settings.

### Open the Project:

1. Open the project in **Visual Studio**.
2. Build the project to restore any NuGet packages or dependencies.

### Run the Application:

1. Press **F5** to run the application in Debug mode.
2. Open a browser and navigate to the provided local server URL to start using the system.

## Usage

### 1. **Login**
   - Users must log in using their company credentials (username and password).
   - Admins have full access, while department employees can only manage reservations within their respective departments.

### 2. **Booking a Meeting Room**
   - Once logged in, users can view the available meeting rooms.
   - Select a room, choose a date and time, and submit the reservation request.
   - Users will be notified whether the booking was successful or not.

### 3. **Report Generation**
   - After the meeting is held, the system can generate a report summarizing the meeting’s results, decisions made, and any follow-up actions required.

### 4. **Admin Features**
   - Admin users have the ability to:
     - View and manage all meeting room reservations.
     - Edit, delete, or approve reservation requests.
     - Manage user roles (e.g., admin, employee).
     - Generate and export reports.

## Database Schema

The database consists of the following main tables:

- **Users**: Stores user credentials, roles, and personal information.
- **MeetingRooms**: Contains information about available meeting rooms (e.g., room name, capacity).
- **Reservations**: Stores meeting room reservations, including date, time, and user details.
- **Reports**: Stores meeting reports generated after the meetings, including results and follow-up actions.

## Future Enhancements

- **Real-time Booking Updates**: Allow for real-time availability updates of rooms (e.g., through WebSockets).
- **Email Notifications**: Send automated email notifications for reservation confirmations and reminders.
- **Mobile Support**: Create a responsive version of the site or a mobile app to allow for easier access on the go.
- **Advanced Reporting**: Add more detailed reporting features with filters for various metrics (e.g., room usage frequency, department-wise bookings).

## Contributing

We welcome contributions to improve the system! If you'd like to contribute, follow these steps:

1. Fork the repository.
2. Create a new branch.
3. Make your changes.
4. Commit your changes.
5. Push your changes to your fork.
6. Open a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgements

- Thanks to the open-source community for various libraries and tools that helped in building this system.
- Special thanks to the SQL Server documentation and Microsoft for the ASP.NET Web Forms framework.

  
## 📬 **Contact**
- **Project Owner:** Ahmed Essam
- **Email:** [ahmedesamo778@gmail.com](mailto:ahmedesamo778@gmail.com)
