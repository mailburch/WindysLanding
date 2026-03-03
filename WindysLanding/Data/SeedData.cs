using Microsoft.EntityFrameworkCore;
using WindysLanding.Models;

namespace WindysLanding.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Animals.Any())
            {
                return; // Database has been seeded
            }

            // ===== ANIMALS =====
            var animals = new Animal[]
            {
                // HORSES
                new Animal
                {
                    Name = "Thunder",
                    AnimalType = "Horse",
                    Age = 8,
                    Size = "Large",
                    Gender = "Gelding",
                    Breed = "Thoroughbred",
                    Color = "Bay",
                    Description = "Thunder is a gentle giant who loves trail rides and has a calm temperament. He's great for intermediate riders and enjoys spending time with people.",
                    Experience = "Intermediate",
                    AdoptionStatus = false,
                    SponsorshipAvailable = true,
                    SponsorshipUrl = "https://zeffy.com/sponsor/thunder"
                },
                new Animal
                {
                    Name = "Willow",
                    AnimalType = "Horse",
                    Age = 5,
                    Size = "Medium",
                    Gender = "Mare",
                    Breed = "Quarter Horse",
                    Color = "Palomino",
                    Description = "Willow is a sweet mare who came to us from a neglect situation. She's made amazing progress and is ready for her forever home with an experienced owner.",
                    Experience = "Experienced",
                    AdoptionStatus = false,
                    SponsorshipAvailable = true,
                    SponsorshipUrl = "https://zeffy.com/sponsor/willow"
                },
                new Animal
                {
                    Name = "Buddy",
                    AnimalType = "Horse",
                    Age = 15,
                    Size = "Medium",
                    Gender = "Gelding",
                    Breed = "Morgan",
                    Color = "Chestnut",
                    Description = "Buddy is a wonderful teacher for beginner riders. He's patient, kind, and loves children. Perfect for a family looking for their first horse.",
                    Experience = "Beginner",
                    AdoptionStatus = false,
                    SponsorshipAvailable = false,
                    SponsorshipUrl = null
                },
                new Animal
                {
                    Name = "Starlight",
                    AnimalType = "Horse",
                    Age = 12,
                    Size = "Large",
                    Gender = "Mare",
                    Breed = "Paint",
                    Color = "Black and White",
                    Description = "Starlight was recently adopted by the Johnson family and is thriving in her new home! She loves her new pasture and her young rider, Emma.",
                    Experience = "Intermediate",
                    AdoptionStatus = true,
                    SponsorshipAvailable = false,
                    SponsorshipUrl = null
                },
                new Animal
                {
                    Name = "Copper",
                    AnimalType = "Horse",
                    Age = 6,
                    Size = "Large",
                    Gender = "Gelding",
                    Breed = "Appaloosa",
                    Color = "Spotted",
                    Description = "Copper is energetic and loves to jump. He needs an active home with someone who can give him the exercise and attention he craves.",
                    Experience = "Advanced",
                    AdoptionStatus = false,
                    SponsorshipAvailable = true,
                    SponsorshipUrl = "https://zeffy.com/sponsor/copper"
                },
                new Animal
                {
                    Name = "Daisy",
                    AnimalType = "Horse",
                    Age = 20,
                    Size = "Small",
                    Gender = "Mare",
                    Breed = "Pony",
                    Color = "Gray",
                    Description = "Daisy is a senior pony who came to us after her owner passed away. She's gentle, loves treats, and would do best as a companion animal.",
                    Experience = "Any",
                    AdoptionStatus = false,
                    SponsorshipAvailable = true,
                    SponsorshipUrl = "https://zeffy.com/sponsor/daisy"
                },
                new Animal
                {
                    Name = "Shadow",
                    AnimalType = "Horse",
                    Age = 10,
                    Size = "Large",
                    Gender = "Gelding",
                    Breed = "Arabian",
                    Color = "Black",
                    Description = "Shadow is a stunning Arabian with lots of personality. He's smart, quick to learn, and bonds deeply with his person.",
                    Experience = "Experienced",
                    AdoptionStatus = false,
                    SponsorshipAvailable = true,
                    SponsorshipUrl = "https://zeffy.com/sponsor/shadow"
                },
                new Animal
                {
                    Name = "Rosie",
                    AnimalType = "Horse",
                    Age = 7,
                    Size = "Medium",
                    Gender = "Mare",
                    Breed = "Tennessee Walker",
                    Color = "Sorrel",
                    Description = "Rosie has the smoothest gait and is perfect for trail riding. She's friendly, curious, and gets along great with other horses.",
                    Experience = "Intermediate",
                    AdoptionStatus = false,
                    SponsorshipAvailable = true,
                    SponsorshipUrl = "https://zeffy.com/sponsor/rosie"
                },

                // DONKEYS
                new Animal
                {
                    Name = "Pedro",
                    AnimalType = "Donkey",
                    Age = 9,
                    Size = "Medium",
                    Gender = "Jack",
                    Breed = "Standard Donkey",
                    Color = "Gray",
                    Description = "Pedro is a friendly donkey who loves attention and treats. He's great with other animals and makes an excellent guardian for livestock.",
                    Experience = "Any",
                    AdoptionStatus = false,
                    SponsorshipAvailable = true,
                    SponsorshipUrl = "https://zeffy.com/sponsor/pedro"
                },
                new Animal
                {
                    Name = "Clementine",
                    AnimalType = "Donkey",
                    Age = 4,
                    Size = "Small",
                    Gender = "Jenny",
                    Breed = "Miniature Donkey",
                    Color = "Brown",
                    Description = "Clementine is a sweet mini donkey who loves kids and attention. She's playful, gentle, and makes everyone smile.",
                    Experience = "Beginner",
                    AdoptionStatus = false,
                    SponsorshipAvailable = true,
                    SponsorshipUrl = "https://zeffy.com/sponsor/clementine"
                },

                // GOAT
                new Animal
                {
                    Name = "Billy",
                    AnimalType = "Goat",
                    Age = 3,
                    Size = "Small",
                    Gender = "Male",
                    Breed = "Nigerian Dwarf",
                    Color = "Multi-colored",
                    Description = "Billy is a playful goat who came to us with Copper. He's friendly, loves to climb, and needs a home with space to explore.",
                    Experience = "Any",
                    AdoptionStatus = false,
                    SponsorshipAvailable = false,
                    SponsorshipUrl = null
                }
            };

            context.Animals.AddRange(animals);
            context.SaveChanges();

            // ===== PHOTOS =====
            var photos = new Photo[]
            {
                new Photo { AnimalId = 1, ImgUrl = "/images/animals/thunder1.jpg", Caption = "Thunder enjoying his pasture" },
                new Photo { AnimalId = 1, ImgUrl = "/images/animals/thunder2.jpg", Caption = "Thunder on a trail ride" },
                new Photo { AnimalId = 2, ImgUrl = "/images/animals/willow1.jpg", Caption = "Willow's beautiful golden coat" },
                new Photo { AnimalId = 3, ImgUrl = "/images/animals/buddy1.jpg", Caption = "Buddy with a young rider" },
                new Photo { AnimalId = 4, ImgUrl = "/images/animals/starlight1.jpg", Caption = "Starlight in her new home" },
                new Photo { AnimalId = 5, ImgUrl = "/images/animals/copper1.jpg", Caption = "Copper showing off his spots" },
                new Photo { AnimalId = 6, ImgUrl = "/images/animals/daisy1.jpg", Caption = "Sweet Daisy" },
                new Photo { AnimalId = 7, ImgUrl = "/images/animals/shadow1.jpg", Caption = "Shadow's striking profile" },
                new Photo { AnimalId = 8, ImgUrl = "/images/animals/rosie1.jpg", Caption = "Rosie on the trail" },
                new Photo { AnimalId = 9, ImgUrl = "/images/animals/pedro1.jpg", Caption = "Pedro the donkey" },
                new Photo { AnimalId = 10, ImgUrl = "/images/animals/clementine1.jpg", Caption = "Clementine being adorable" },
                new Photo { AnimalId = 11, ImgUrl = "/images/animals/billy1.jpg", Caption = "Billy the goat" }
            };

            context.Photos.AddRange(photos);
            context.SaveChanges();

            // ===== SUCCESS STORIES =====
            var successStories = new SuccessStory[]
            {
                new SuccessStory
                {
                    AnimalId = 4, // Starlight
                    Title = "Starlight Finds Her Perfect Match",
                    Content = "When Starlight came to Windy's Landing, she was nervous and unsure. After months of patience and care, she blossomed into a confident mare. The Johnson family fell in love with her gentle nature, and now Starlight has a forever home where she's loved and cherished. Young Emma rides her every weekend, and they've become the best of friends!",
                    DatePublished = DateTime.Now.AddMonths(-2)
                },
                new SuccessStory
                {
                    AnimalId = 4,
                    Title = "From Rescue to Ribbon Winner",
                    Content = "We're so proud of Starlight! Just six months after her adoption, she and Emma competed in their first local show and won a blue ribbon in the trail class. It's amazing to see how far this beautiful mare has come from her rescue days.",
                    DatePublished = DateTime.Now.AddMonths(-1)
                }
            };

            context.SuccessStories.AddRange(successStories);
            context.SaveChanges();

            // ===== EVENTS =====
            var events = new Event[]
            {
                new Event
                {
                    Name = "Spring Open House",
                    Description = "Join us for our annual Spring Open House! Meet our animals, tour the facility, learn about volunteer opportunities, and enjoy refreshments. Family-friendly event!",
                    Date = DateTime.Now.AddMonths(2),
                    EventUrl = "https://facebook.com/events/springopenhouse",
                    EventImg = "/images/events/spring-open-house.jpg"
                },
                new Event
                {
                    Name = "Sunset Trail Ride Fundraiser",
                    Description = "Enjoy a beautiful sunset trail ride through our property. All proceeds benefit the animals at Windy's Landing. Advanced registration required. Riders must be intermediate level or above.",
                    Date = DateTime.Now.AddMonths(1),
                    EventUrl = "https://zeffy.com/events/sunset-ride",
                    EventImg = "/images/events/sunset-ride.jpg"
                },
                new Event
                {
                    Name = "Volunteer Orientation",
                    Description = "Interested in volunteering? Join us for an orientation session where you'll learn about our mission, meet the team, and discover how you can help make a difference in the lives of rescued animals.",
                    Date = DateTime.Now.AddDays(15),
                    EventUrl = null,
                    EventImg = "/images/events/volunteer-orientation.jpg"
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();

            // ===== FAQs =====
            var faqs = new FAQ[]
            {
                new FAQ
                {
                    Question = "What is the adoption process?",
                    Answer = "Our adoption process includes: 1) Submit an application, 2) Home visit and interview, 3) Meet the animal, 4) Trial period (if applicable), 5) Finalize adoption with contract and fee. We want to ensure every animal finds the perfect forever home!",
                    Category = "Adoption",
                    DisplayOrder = 1
                },
                new FAQ
                {
                    Question = "How much does it cost to adopt?",
                    Answer = "Adoption fees vary by animal and typically range from $200-$800. This helps cover veterinary care, feed, and shelter costs. All animals are up-to-date on vaccinations and have received necessary medical care before adoption.",
                    Category = "Adoption",
                    DisplayOrder = 2
                },
                new FAQ
                {
                    Question = "Can I volunteer if I don't have horse experience?",
                    Answer = "Absolutely! We have opportunities for all skill levels. Tasks include: facility maintenance, fundraising help, social media, administrative work, and more. We'll train you on animal care if you're interested in working directly with our rescues.",
                    Category = "Volunteering",
                    DisplayOrder = 3
                },
                new FAQ
                {
                    Question = "What does sponsoring an animal involve?",
                    Answer = "When you sponsor an animal, your monthly donation directly supports their care including feed, veterinary expenses, and shelter. You'll receive updates and photos of your sponsored animal. It's a wonderful way to help if you can't adopt!",
                    Category = "Sponsorship",
                    DisplayOrder = 4
                },
                new FAQ
                {
                    Question = "Do you accept donations of supplies?",
                    Answer = "Yes! We always need hay, grain, supplements, grooming supplies, and barn equipment. Check our Amazon and Chewy wishlists on our website for specific needs. Please contact us before dropping off large items.",
                    Category = "Donations",
                    DisplayOrder = 5
                }
            };

            context.FAQs.AddRange(faqs);
            context.SaveChanges();

            // ===== NEWSLETTERS =====
            var newsletters = new Newsletter[]
            {
                new Newsletter
                {
                    Title = "Winter 2024 Newsletter",
                    NewsletterUrl = "/pdfs/newsletters/winter-2024.pdf",
                    PublishDate = DateTime.Now.AddMonths(-3)
                },
                new Newsletter
                {
                    Title = "Spring 2025 Newsletter",
                    NewsletterUrl = "/pdfs/newsletters/spring-2025.pdf",
                    PublishDate = DateTime.Now.AddMonths(-1)
                }
            };

            context.Newsletters.AddRange(newsletters);
            context.SaveChanges();

            // ===== SPONSOR COMPANIES =====
            var sponsorCompanies = new SponsorCompany[]
            {
                new SponsorCompany
                {
                    Name = "Traverse City Feed & Supply",
                    Description = "Local feed store supporting Windy's Landing since 2020. Visit them at 123 Main St, Traverse City. www.tcfeed.com"
                },
                new SponsorCompany
                {
                    Name = "Northern Michigan Veterinary Clinic",
                    Description = "Providing compassionate veterinary care for all our rescues. Dr. Sarah Johnson and team have been invaluable partners. www.nmvetclinic.com"
                },
                new SponsorCompany
                {
                    Name = "Great Lakes Equine Insurance",
                    Description = "Proud to support animal rescue in our community. Protecting horses and their owners across Michigan. www.gleinsurance.com"
                },
                new SponsorCompany
                {
                    Name = "Cherry Capital Hay Company",
                    Description = "Premium hay supplier and long-time supporter of Windy's Landing Animal Rescue. Family-owned since 1985."
                }
            };

            context.SponsorCompanies.AddRange(sponsorCompanies);
            context.SaveChanges();

            // ===== VOLUNTEER APPLICATIONS =====
            var volunteerApplications = new VolunteerApplication[]
            {
                new VolunteerApplication
                {
                    Name = "Sarah Mitchell",
                    Email = "sarah.mitchell@email.com",
                    Phone = "231-555-0123",
                    VolunteerDescription = "I've always loved horses and would love to help with feeding, grooming, and general care. I'm available on weekends.",
                    ApplicationDate = DateTime.Now.AddDays(-5),
                    WaiverSigned = true,
                    WaiverSignedDate = DateTime.Now.AddDays(-5)
                },
                new VolunteerApplication
                {
                    Name = "Mike Chen",
                    Email = "m.chen@email.com",
                    Phone = "231-555-0456",
                    VolunteerDescription = "I'm a retired carpenter and would be happy to help with facility maintenance and repairs.",
                    ApplicationDate = DateTime.Now.AddDays(-2),
                    WaiverSigned = false,
                    WaiverSignedDate = null
                }
            };

            context.VolunteerApplications.AddRange(volunteerApplications);
            context.SaveChanges();

            // ===== CONTACT INFO =====
            var contactInfo = new ContactInfo
            {
                Phone = "231-555-7890",
                Email = "info@windyslanding.org",
                Address = "1234 Country Road, Traverse City, MI 49684"
            };

            context.ContactInfos.Add(contactInfo);
            context.SaveChanges();

            // ===== USERS (Example users) =====
            var users = new User[]
            {
                new User
                {
                    Name = "Emily Johnson",
                    Email = "emily.johnson@gmail.com",
                    NewsletterOptIn = true
                },
                new User
                {
                    Name = "David Thompson",
                    Email = "d.thompson@yahoo.com",
                    NewsletterOptIn = false
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
