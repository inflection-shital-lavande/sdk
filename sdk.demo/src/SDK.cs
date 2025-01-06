using sdk.demo.src.api.action_plan.ActionPlanService;
using sdk.demo.src.api.animation.AnimationService;
using sdk.demo.src.api.appointment.AppointmentService;
using sdk.demo.src.api.user.UserService;

namespace sdk.demo.SDK;
public class SDK
{
    public APIClient Client { get; set; }
    public User User { get; set; }
    public ActionPlan ActionPlan { get; set; }
    public Animation Animation { get; set; }
    public Appointment Appointment { get; set; }
    public SDK(string baseUrl)
    {
        Client = new APIClient(baseUrl);
        User = new User(Client);
        ActionPlan = new ActionPlan(Client);
        Animation = new Animation(Client);
        Appointment = new Appointment(Client);

    }
}



