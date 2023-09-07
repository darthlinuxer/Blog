$JWTTokenPwd="Very_Long_Jwt_Token_Password_To_Be_Used_For_Validation"
New-Item -Path "env:\JWTTokenPwd" -Value $JWTTokenPwd
dotnet run --project src/webapi