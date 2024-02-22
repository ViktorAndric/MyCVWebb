using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MongoDB.Bson.IO;
using System.Net.Http;
using System.Net.Http.Json;
using MyCVWebb.Library.Data;
using MyCVWebb.Library.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.'
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

MongoDb<About> AboutDB = new MongoDb<About>("MyCv");
MongoDb<Contact> ContactDB = new MongoDb<Contact>("MyCv");
MongoDb<Education> EducationDB = new MongoDb<Education>("MyCv");
MongoDb<Project> ProjectDB = new MongoDb<Project>("MyCv");
MongoDb<Skills> SkillsDB = new MongoDb<Skills>("MyCv");
MongoDb<WorkExperience> WorkExperienceDB = new MongoDb<WorkExperience>("MyCv");


builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();



//****************************************************************************
//Qoute- 
app.MapGet("/qoute", async () =>
{
	var client = new HttpClient();
	{
		try
		{
			var response = await client.GetAsync("https://api.adviceslip.com/advice");
			var content = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				var qouteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<QouteResponse>(content);
				var qoute = qouteResponse.Slip;
				return Results.Ok(qoute);
			}
			else
			{
				return Results.NotFound("No qoute for you!");
			}
		}
		catch
		{
			return Results.NotFound("No qoute for you!");
		}
	}

});
//***********************************************************************************
//About
//C
app.MapPost("/about", async (About about) =>
{
	var aboutMe = await AboutDB.AddAsync("AboutMe", about);
	return Results.Ok(aboutMe);

});

//R
app.MapGet("/about", async () =>
{
	var aboutMeList = await AboutDB.GetAllAsync("AboutMe");
	var aboutMe = aboutMeList.FirstOrDefault();
	if (aboutMe is not null)
	{
		return Results.Ok(aboutMe);
	}
	return Results.NotFound("About me not found...");
});

//U
app.MapPut("/about/{id}", async (string id, About about) =>
{
	var result = await AboutDB.UpdateAsync<About>(id, "AboutMe", about);

	if (result is not null)
	{
		return Results.Ok(result);
	}

	return Results.NotFound("Not found...");
});

//D
app.MapDelete("/about", async (string id) =>
{
	var about = await AboutDB.DeleteAsync<About>("AboutMe", id);
	return Results.Ok(about);
});


//***********************************************************************************

//***********************************************************************************
//Contact
//C
app.MapPost("/contact", async (Contact contact) =>
{
	var newContact = await ContactDB.AddAsync("Contact", contact);
	return Results.Ok(newContact);

});

//R all
app.MapGet("/contacts", async () =>
{
	var newContact = await ContactDB.GetAllAsync("Contact");
	if (newContact is not null)
	{
		return Results.Ok(newContact);
	}

	return Results.NotFound("Not found...");
});

//R by id
app.MapGet("/contact/{id}", async (string id) =>
{
	var contact = await ContactDB.GetByIDAsync<Contact>(id, "Contact");
	if (contact is not null)
	{
		return Results.Ok(contact);
	}

	return Results.NotFound("Not found...");
});

//U
app.MapPut("/contact/{id}", async (string id, Contact contact) =>
{
	var result = await ContactDB.UpdateAsync<Contact>(id, "Contact", contact);

	if (result is not null)
	{
		return Results.Ok(result);
	}

	return Results.NotFound("Not found...");
});

//D
app.MapDelete("/contact/{id}", async (string id) =>
{
	var contact = await ContactDB.DeleteAsync<Contact>("Contact", id);
	return Results.Ok(contact);
});


//***********************************************************************************
//***********************************************************************************
//Education
//C
app.MapPost("/education", async (Education education) =>
{
	var newEducation = await EducationDB.AddAsync("Education", education);
	return Results.Ok(newEducation);

});

//R all
app.MapGet("/educations", async () =>
{
	var allEducations = await EducationDB.GetAllAsync("Education");
	return Results.Ok(allEducations);
});

//R by id
app.MapGet("/education/{id}", async (string id) =>
{
	var education = await EducationDB.GetByIDAsync<Education>(id, "Education");
	return Results.Ok(education);
});

//U
app.MapPut("/education{id}", async (string id, Education education) =>
{
	var result = await EducationDB.UpdateAsync<Education>(id, "Education", education);

	if (result is not null)
	{
		return Results.Ok(result);
	}

	return Results.NotFound("Not found...");
});

//D
app.MapDelete("/education/{id}", async (string id) =>
{
	var education = await EducationDB.DeleteAsync<Education>("Education", id);
	return Results.Ok(education);
});


//***********************************************************************************
//***********************************************************************************
//Project
//C
app.MapPost("/project", async (Project project) =>
{
	var newProject = await ProjectDB.AddAsync("Project", project);
	return Results.Ok(newProject);

});

//R all
app.MapGet("/projects", async () =>
{
	var allProjects = await ProjectDB.GetAllAsync("Project");
	return Results.Ok(allProjects);
});

//R by id
app.MapGet("/project/{id}", async (string id) =>
{
	var project = await ProjectDB.GetByIDAsync<Project>(id, "Project");
	return Results.Ok(project);
});

//U
app.MapPut("/project/{id}", async (string id, Project project) =>
{
	var result = await ProjectDB.UpdateAsync<Project>(id, "Project", project);

	if (result is not null)
	{
		return Results.Ok(result);
	}

	return Results.NotFound("Not found...");
});

//D
app.MapDelete("/project/{id}", async (string id) =>
{
	var project = await ProjectDB.DeleteAsync<Project>("Project", id);
	return Results.Ok(project);
});


//***********************************************************************************
//***********************************************************************************
//Categories
//C
app.MapPost("/skill", async (Skills skill) =>
{
	var newSkill = await SkillsDB.AddAsync("Skill", skill);
	return Results.Ok(newSkill);

});

//R all
app.MapGet("/skills", async () =>
{
	var allSkills = await SkillsDB.GetAllAsync("Skill");
	return Results.Ok(allSkills);
});

//R by id
app.MapGet("/skill/{id}", async (string id) =>
{
	var skill = await SkillsDB.GetByIDAsync<Skills>(id, "Skill");
	return Results.Ok(skill);
});

//U
app.MapPut("/skill/{id}", async (string id, Skills skill) =>
{
	var result = await SkillsDB.UpdateAsync<Skills>(id, "Skill", skill);

	if (result is not null)
	{
		return Results.Ok(result);
	}

	return Results.NotFound("Not found...");
});

//D
app.MapDelete("/skill/{id}", async (string id) =>
{
	var skill = await SkillsDB.DeleteAsync<Skills>("Skill", id);
	return Results.Ok(skill);
});


//***********************************************************************************
//***********************************************************************************
//Categories
//C
app.MapPost("/workexperience", async (WorkExperience workE) =>
{
	var newExperience = await WorkExperienceDB.AddAsync("Experience", workE);
	return Results.Ok(newExperience);

});

//R all
app.MapGet("/workexperiences", async () =>
{
	var allExperiences = await WorkExperienceDB.GetAllAsync("Experience");
	return Results.Ok(allExperiences);
});

//R by id
app.MapGet("/workexperience/{id}", async (string id) =>
{
	var experience = await WorkExperienceDB.GetByIDAsync<WorkExperience>(id, "Experience");
	if (experience is not null)
	{
		return Results.Ok(experience);
	}

	return Results.NotFound("Not found...");
});

//U
app.MapPut("/workexperience/{id}", async (string id, WorkExperience workE) =>
{
	var result = await WorkExperienceDB.UpdateAsync<WorkExperience>(id, "Experience", workE);

	if (result is not null)
	{
		return Results.Ok(result);
	}

	return Results.NotFound("Not found...");
});

//D
app.MapDelete("/workexperience/{id}", async (string id) =>
{
	var experience = await WorkExperienceDB.DeleteAsync<WorkExperience>("Experience", id);
	return Results.Ok(experience);
});


//***********************************************************************************

app.Run();