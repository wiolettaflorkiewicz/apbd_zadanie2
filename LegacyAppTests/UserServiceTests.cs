using LegacyApp;

namespace LegacyAppTests;

public class UserServiceTests
{
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Without_At_And_Dot()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Doe";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        string email = "doe";
        var service = new UserService();
        
        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        // Assert
        Assert.Equal(false, result);

    }

    [Fact]
    public void AddUser_Should_Return_False_When_Missing_FirstName()
    {
        // Arrange
        var service = new UserService();
        
        //Act
        var result = service.AddUser(null, "Doe", "doe@wp.pl", new DateTime(1990, 1, 1), 1);
        
        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Missing_LastName()
    {
        // Arrange
        var service = new UserService();
        
        //Act
        var result = service.AddUser("John", null, "doe@wp.pl", new DateTime(1990, 1, 1), 1);
        
        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Age_Is_Less_Than_21()
    {
        // Arrange
        var service = new UserService();
        var today = DateTime.Today;
        var justUnder21YearsAgo = today.AddYears(-21).AddDays(1);

        // Act
        var result = service.AddUser("John", "Doe", "doe@wp.pl", justUnder21YearsAgo, 1);

        // Assert
        Assert.False(result, "AddUser should return false when age is less than 21.");
    }
    

    [Fact]
    public void AddUser_Should_Return_True_When_User_Is_Very_Important_Client()
    {
        // Arrange
        var service = new UserService();
        
        //Act
        var result = service.AddUser("John", "Doe", "doe@wp.pl", new DateTime(1990, 1, 1), 2);

        // Assert
        Assert.Equal(true, result);
        
    }
    
    [Fact]
    public void AddUser_Should_Return_True_When_User_Is_Important_Client()
    {
        // Arrange
        var service = new UserService();
        
        //Act
        var result = service.AddUser("John", "Doe", "doe@wp.pl", new DateTime(1990, 1, 1), 3);

        // Assert
        Assert.Equal(true, result);
        
    }

    [Fact]
    public void AddUser_Should_Return_True_When_User_Normal_Client()
    {
        // Arrange
        var service = new UserService();
        
        //Act
        var result = service.AddUser("John", "Doe", "doe@wp.pl", new DateTime(1990, 1, 1), 5);

        // Assert
        Assert.Equal(true, result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_User_Has_Credit_Limit_Under_500()
    {
        //Arrange
        var service = new UserService();

        //Act
        var result = service.AddUser("John", "Kowalski", "kowalski@wp.pl", new DateTime(1990, 1, 1), 1);

        //Assert
        Assert.Equal(false, result);
    }
    
    [Fact]
    public void AddUser_Should_Throw_Exception_When_User_Does_Not_Exist()
    {
        //Arrange
        var service = new UserService();

        //Act and Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _ = service.AddUser("John", "Johny", "johnyjohn@wp.pl", new DateTime(1980, 1, 1), 100);
        });
    }
    
    [Fact]
    public void AddUser_Should_Throw_Exception_When_User_No_Credit_Limit_Exists_For_User()
    {
        //Arrange
        var service = new UserService();

        //Act and Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _ = service.AddUser("John", "Johny", "john@wp.pl", new DateTime(1980, 1, 1), 6);
        });
    }

}