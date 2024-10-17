docker-compose up -d

dotnet-ef database drop -f -c WriteDbContext 		-p .\src\PetManagement\PetHouse.PetManagement.Infrastructure\ 		            -s .\src\PetHouse.Web\
dotnet-ef database drop -f -c WriteDbContext 	-p .\src\SpecieManagement\PetHouse.SpecieManagement.Infrastructure\ 		            -s .\src\PetHouse.Web\
dotnet-ef database drop -f -c AccountsDbContext 	-p .\src\Accounts\PetHouse.Accounts.Infrastructure\ 		        -s .\src\PetHouse.Web\


dotnet-ef migrations remove -c WriteDbContext 		-p .\src\PetManagement\PetHouse.PetManagement.Infrastructure\ 		            -s .\src\PetHouse.Web\
dotnet-ef migrations remove -c WriteDbContext 	-p .\src\SpecieManagement\PetHouse.SpecieManagement.Infrastructure\ 		            -s .\src\PetHouse.Web\
dotnet-ef migrations remove -c AccountsDbContext 	-p .\src\Accounts\PetHouse.Accounts.Infrastructure\ 		        -s .\src\PetHouse.Web\


dotnet-ef migrations add PetManagement_init        -c WriteDbContext 	-p .\src\PetManagement\PetHouse.PetManagement.Infrastructure\ 		                -s .\src\PetHouse.Web\
dotnet-ef migrations add SpecieManagement_init         -c WriteDbContext 	-p .\src\SpecieManagement\PetHouse.SpecieManagement.Infrastructure\ 		            -s .\src\PetHouse.Web\
dotnet-ef migrations add Accounts_init         -c AccountsDbContext 	-p .\src\Accounts\PetHouse.Accounts.Infrastructure\ 		        -s .\src\PetHouse.Web\


dotnet-ef database update -c WriteDbContext 		-p .\src\PetManagement\PetHouse.PetManagement.Infrastructure\ 		            -s .\src\PetHouse.Web\
dotnet-ef database update -c WriteDbContext 	-p .\src\SpecieManagement\PetHouse.SpecieManagement.Infrastructure\ 		            -s .\src\PetHouse.Web\
dotnet-ef database update -c AccountsDbContext 		-p .\src\Accounts\PetHouse.Accounts.Infrastructure\ 		        -s .\src\PetHouse.Web\


pause