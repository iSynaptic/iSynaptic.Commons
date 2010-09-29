using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class EquatableTests
    {
        #region Test Types

        public struct PersonStats
        {
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
        }

        public interface IAmEquatable : IEquatable<IAmEquatable>
        {
        }

        public class WithInterfaceReferences
        {
            public IAmEquatable Reference { get; set; }
        }

        private abstract class Address : IEquatable<Address>
        {
            public Address(string purpose)
            {
                Purpose = purpose;
            }

            public string Purpose { get; private set; }

            #region Value Equivelence Implementation

            public bool Equals(Address other) { return this.IsEqualsTo(other); }
            public override bool Equals(object obj) { return this.IsEqualsTo(obj as Address); }

            public override int GetHashCode() { return this.ToHashCode(); }

            public static bool operator ==(Address left, Address right) { return left.IsEqualsTo(right); }
            public static bool operator !=(Address left, Address right) { return !(left == right); }

            #endregion
        }

        private class EmailAddress : Address, IEquatable<EmailAddress>
        {
            public EmailAddress(string address, int maxEmailsPerDay, string purpose) : base(purpose)
            {
                Address = address;
                MaxEmailsPerDay = maxEmailsPerDay;
            }

            public string Address { get; private set; }
            public int MaxEmailsPerDay { get; private set; }

            #region Value Equivelence Implementation

            public bool Equals(EmailAddress other) { return this.IsEqualsTo(other); }
            public override bool Equals(object obj) { return this.IsEqualsTo(obj as EmailAddress); }

            public override int GetHashCode() { return this.ToHashCode(); }

            public static bool operator ==(EmailAddress left, EmailAddress right) { return left.IsEqualsTo(right); }
            public static bool operator !=(EmailAddress left, EmailAddress right) { return !(left == right); }

            #endregion
        }

        private class GeographicAddress : Address, IEquatable<GeographicAddress>
        {
            public GeographicAddress(string street, string city, string state, string postalCode, string purpose) : base(purpose)
            {
                Street = street;
                City = city;
                State = state;
                PostalCode = postalCode;
            }

            public string Street { get; private set; }
            public string City { get; private set; }
            public string State { get; private set; }
            public string PostalCode { get; private set; }

            #region Value Equivelence Implementation

            public bool Equals(GeographicAddress other) { return this.IsEqualsTo(other); }
            public override bool Equals(object obj) { return this.IsEqualsTo(obj as GeographicAddress); }

            public override int GetHashCode() { return this.ToHashCode(); }

            public static bool operator ==(GeographicAddress left, GeographicAddress right) { return left.IsEqualsTo(right); }
            public static bool operator !=(GeographicAddress left, GeographicAddress right) { return !(left == right); }

            #endregion
        }

        private class PrimaryContactInfo : IEquatable<PrimaryContactInfo>
        {
            public PrimaryContactInfo(GeographicAddress geoAddress, EmailAddress emailAddress)
            {
                GeoAddress = geoAddress;
                EmailAddress = emailAddress;
            }

            public GeographicAddress GeoAddress { get; private set; }
            public EmailAddress EmailAddress { get; private set; }

            #region Value Equivelence Implementation

            public bool Equals(PrimaryContactInfo other) { return this.IsEqualsTo(other); }
            public override bool Equals(object obj) { return this.IsEqualsTo(obj as PrimaryContactInfo); }

            public override int GetHashCode() { return this.ToHashCode(); }

            public static bool operator ==(PrimaryContactInfo left, PrimaryContactInfo right) { return left.IsEqualsTo(right); }
            public static bool operator !=(PrimaryContactInfo left, PrimaryContactInfo right) { return !(left == right); }

            #endregion

        }

        #endregion

        [Test]
        public void IsEqualsTo_WithTwoNulls_ReturnsTrue()
        {
            Assert.IsTrue(Equatable<string>.IsEqualsTo(null, null));
        }

        [Test]
        public void IsEqualsTo_WithOneNull_ReturnsFalse()
        {
            Assert.IsFalse(Equatable<string>.IsEqualsTo("Foo", null));
            Assert.IsFalse(Equatable<string>.IsEqualsTo(null, "Foo"));
        }

        [Test]
        public void ToHashCode_WithNull_ReturnsZero()
        {
            Assert.AreEqual(0, Equatable<string>.ToHashCode(null));
        }

        [Test]
        [Ignore]
        public void IsEqualsTo_WhenExactSameReference_ReturnsTrue()
        {
            var address = new EmailAddress("foo@bar.com", 1, "General Communication");

            Assert.IsTrue(address.Equals(address));
        }

        [Test]
        [Ignore]
        public void IsEqualsTo_WhenSameData_ReturnsTrue()
        {
            var address1 = new EmailAddress("foo@bar.com", 1, "General Communication");
            var address2 = new EmailAddress("foo@bar.com", 1, "General Communication");

            Assert.IsTrue(address1.Equals(address2));
        }

        [Test]
        [Ignore]
        public void ToHashCode_WhenSameData_ReturnsSameValue()
        {
            var address1 = new EmailAddress("foo@bar.com", 1, "General Communication");
            var address2 = new EmailAddress("foo@bar.com", 1, "General Communication");

            Assert.AreEqual(address1.GetHashCode(), address2.GetHashCode());
        }

        [Test]
        [Ignore]
        public void IsEqualsTo_WhenOneDifferenceInPrimativeValue_ReturnsFalse()
        {
            var address1 = new EmailAddress("foo@bar.com", 1, "General Communication");
            var address2 = new EmailAddress("foo@bar.com", 2, "General Communication");

            Assert.IsFalse(address1.Equals(address2));
        }

        [Test]
        [Ignore]
        public void ToHashCode_WhenOneDifferenceInPrimativeValue_ReturnsDifferentValue()
        {
            var address1 = new EmailAddress("foo@bar.com", 1, "General Communication");
            var address2 = new EmailAddress("foo@bar.com", 2, "General Communication");

            Assert.AreNotEqual(address1.GetHashCode(), address2.GetHashCode());
        }

        [Test]
        [Ignore]
        public void IsEqualTo_WhenSameDataInComplexType_ReturnsTrue()
        {
            var geoAddress1 = new GeographicAddress("123 Main Street", "Saint Paul", "MN", "55110", "Primary");
            var geoAddress2 = new GeographicAddress("123 Main Street", "Saint Paul", "MN", "55110", "Primary");

            var emailAddress1 = new EmailAddress("foo@bar.com", 1, "Primary");
            var emailAddress2 = new EmailAddress("foo@bar.com", 1, "Primary");

            var primaryContactInfo1 = new PrimaryContactInfo(geoAddress1, emailAddress1);
            var primaryContactInfo2 = new PrimaryContactInfo(geoAddress2, emailAddress2);

            Assert.IsTrue(primaryContactInfo1.Equals(primaryContactInfo2));
        }

        [Test]
        [Ignore]
        public void ToHashCode_WhenSameDataInComplexType_ReturnsSameValue()
        {
            var geoAddress1 = new GeographicAddress("123 Main Street", "Saint Paul", "MN", "55110", "Primary");
            var geoAddress2 = new GeographicAddress("123 Main Street", "Saint Paul", "MN", "55110", "Primary");

            var emailAddress1 = new EmailAddress("foo@bar.com", 1, "Primary");
            var emailAddress2 = new EmailAddress("foo@bar.com", 1, "Primary");

            var primaryContactInfo1 = new PrimaryContactInfo(geoAddress1, emailAddress1);
            var primaryContactInfo2 = new PrimaryContactInfo(geoAddress2, emailAddress2);

            Assert.AreEqual(primaryContactInfo1.GetHashCode(), primaryContactInfo2.GetHashCode());
        }

        [Test]
        [Ignore]
        public void IsEqualTo_WhenOneDifferenceInPrimativeValueInComplexType_ReturnsTrue()
        {
            var geoAddress1 = new GeographicAddress("123 Main Street", "Saint Paul", "MN", "55110", "Primary");
            var geoAddress2 = new GeographicAddress("123 Main Street", "Saint Paul", "MN", "55110", "Primary");

            var emailAddress1 = new EmailAddress("foo@bar.com", 1, "Primary");
            var emailAddress2 = new EmailAddress("foo@bar.com", 2, "Primary");

            var primaryContactInfo1 = new PrimaryContactInfo(geoAddress1, emailAddress1);
            var primaryContactInfo2 = new PrimaryContactInfo(geoAddress2, emailAddress2);

            Assert.IsFalse(primaryContactInfo1.Equals(primaryContactInfo2));
        }

        [Test]
        [Ignore]
        public void ToHashCode_WhenOneDifferenceInPrimativeValueInComplexType_ReturnsDifferentValue()
        {
            var geoAddress1 = new GeographicAddress("123 Main Street", "Saint Paul", "MN", "55110", "Primary");
            var geoAddress2 = new GeographicAddress("123 Main Street", "Saint Paul", "MN", "55110", "Primary");

            var emailAddress1 = new EmailAddress("foo@bar.com", 1, "Primary");
            var emailAddress2 = new EmailAddress("foo@bar.com", 2, "Primary");

            var primaryContactInfo1 = new PrimaryContactInfo(geoAddress1, emailAddress1);
            var primaryContactInfo2 = new PrimaryContactInfo(geoAddress2, emailAddress2);

            Assert.AreNotEqual(primaryContactInfo1.GetHashCode(), primaryContactInfo2.GetHashCode());
        }

        [Test]
        [Ignore]
        public void IsEqualTo_WhenSameDataInStruct_ReturnsTrue()
        {
            var now = DateTime.Now;

            var personStats1 = new PersonStats { DateOfBirth = now, Name = "John Smith" };
            var personStats2 = new PersonStats { DateOfBirth = now, Name = "John Smith" };

            Assert.IsTrue(personStats1.IsEqualsTo(personStats2));
        }

        [Test]
        [Ignore]
        public void IsEqualTo_WhenDifferentDataInStruct_ReturnsFalse()
        {
            var now = DateTime.Now;

            var personStats1 = new PersonStats { DateOfBirth = now, Name = "John Smith" };
            var personStats2 = new PersonStats { DateOfBirth = now.AddDays(1), Name = "Jane Doe" };

            Assert.IsFalse(personStats1.IsEqualsTo(personStats2));
        }

        [Test]
        [Ignore]
        public void IsEqualsTo_WhenEvaluatingAFieldThatImplementsIEquatable_DirectlyUsesEqualsMethodViaInterface()
        {
            var mocks = new MockRepository();

            var equatable = mocks.StrictMock<IAmEquatable>();
            equatable.Expect(x => x.Equals(equatable));

            var left = new WithInterfaceReferences { Reference = equatable };
            var right = new WithInterfaceReferences { Reference = equatable };

            Assert.IsTrue(left.IsEqualsTo(right));

            mocks.VerifyAll();
        }
    }
}
