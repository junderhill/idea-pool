using System;
using FluentValidation.TestHelper;
using MyIdeaPool.Validators;
using Xunit;

namespace IdeaPool.Tests
{
    public class UserSignupViewModelValidatorTests
    {
        private readonly UserSignupViewModelValidator sut;

        public UserSignupViewModelValidatorTests()
        {
           sut = new UserSignupViewModelValidator(); 
        }

        [Fact]
        public void TestThatPasswordWithLessThan8CharactersIsNotValid()
        {
            sut.ShouldHaveValidationErrorFor(x => x.password, "Abc1234");
        }

        [Fact]
        public void TestThatPasswordWith8CharactersIsValid()
        {
            sut.ShouldNotHaveValidationErrorFor(x => x.password, "Abc123De");
        }

        [Fact]
        public void TestThatPasswordWithoutUppercaseIsNotValid()
        {
            sut.ShouldHaveValidationErrorFor(x => x.password, "abc123def");
        }

        [Fact]
        public void TestThatPasswordWithUppercaseIsValid()
        {
            sut.ShouldNotHaveValidationErrorFor(x => x.password, "abc123Def");
        }

        [Fact]
        public void TestThatPasswordWithoutLowercaseIsNotValid()
        {
            sut.ShouldHaveValidationErrorFor(x => x.password, "ABC123DEF");
        }

        [Fact]
        public void TestThatPasswordWithLowercaseIsValid()
        {
            sut.ShouldNotHaveValidationErrorFor(x => x.password, "abc123DEF");
        }

        [Fact]
        public void TestThatPasswordWithoutNumberIsNotValid()
        {
            sut.ShouldHaveValidationErrorFor(x => x.password, "ABCdefGHI");
        }

        [Fact]
        public void TestThatPasswordWithNumberIsValid()
        {
            sut.ShouldNotHaveValidationErrorFor(x => x.password, "abcDEFg1");
        }

        [Fact]
        public void TestThatEmptyNameIsNotValid()
        {
            sut.ShouldHaveValidationErrorFor(x => x.name, String.Empty);
        }
        
        [Fact]
        public void TestThatCompletedNameIsValid()
        {
            sut.ShouldNotHaveValidationErrorFor(x => x.name, "Jason Underhill");
        }
        [Fact]
        public void TestThatEmptyEmailIsNotValid()
        {
            sut.ShouldHaveValidationErrorFor(x => x.email, String.Empty);
        }
        
        [Fact]
        public void TestThatPopulatedEmailDressIsValid()
        {
            sut.ShouldNotHaveValidationErrorFor(x => x.email, "jason.underhill@test.com");
        }
    }
}
