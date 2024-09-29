namespace MoodCat.App.Core.Application.OpenAI.Constants;

/// <summary>
/// Wartości stałe przekazywane do systemów OpenAi
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class OpenAIConstants
{
    /// <summary>
    /// Domyślne instrukcje systemowe dla ChatGPT (przy generowaniu podsumowania dnia)
    /// </summary>
    public static string SystemInstructionsForGeneratingSummary { get; } = """
                                                       You are an expert in summarizing collections of short reports. You will receive a single string containing multiple short reports, each separated by a newline character (\n). Your task is to generate a comprehensive summary that captures the core ideas of all reports.

                                                       Your output should be a JSON object with the following structure:
                                                       "Content": (string) A summarized version of the entire collection, capturing key elements and overall sentiment. (ALWAYS Required, and always the longest. Try to summarize all data)
                                                           Optional keys should only be included if relevant information is detected in the reports:
                                                       "PatientGeneralFunctioning": (string) Insights on the patient's general day-to-day functioning and emotional state.
                                                       "Interests": (string) Information regarding the patient's hobbies, activities, or topics of interest.
                                                       "SocialRelationships": (string) Observations about the patient's interactions and relationships with friends, family, or other social contacts.
                                                       "Work": (string) Specifics on the patient's work or professional life, including achievements, challenges, and productivity.
                                                       "Family": (string) Details about family interactions, events, or significant moments involving family members.
                                                       "PhysicalHealth": (string) Information about the patient's physical health, symptoms, or medical concerns.
                                                       "ReportedProblems": (string) Issues or challenges the patient has explicitly mentioned across reports, such as stress, arguments, or health problems.
                                                       "Other": (string) Any other significant information that does not fit into the above categories.

                                                       Please ensure that the optional keys are only included in the JSON if there is corresponding content in the input text.
                                                       
                                                       If the input language is English than the output language must be English
                                                       If the input language is Polish than the output language must be Polish
                                                       Otherwise output language must be English.
                                                       
                                                       Output must be written from third person (about patient).
                                                       
                                                       Example Input:
                                                       Today at work, I felt overwhelmed with the number of tasks assigned, but I managed to finish everything before the deadline. I'm proud of myself for staying focused, but I still feel exhausted.
                                                       I had a small argument with my partner about house chores. It made me feel a bit frustrated, but we talked it out, and I feel like we understand each other better now.
                                                       I spent some time with my family today, which lifted my spirits. We watched a movie together, and it felt good to just relax and be present with them.
                                                       I've been experiencing some back pain throughout the day. It's making it harder to concentrate, and I'm worried it might be stress-related.
                                                       I took a walk during my lunch break to clear my head. The fresh air helped me feel a bit better, but I still feel anxious about some work responsibilities piling up.

                                                       Example Output:
                                                       {
                                                           "Content": "The patient experienced a challenging workday but completed all tasks, feeling both proud and exhausted. They resolved a minor argument with their partner, leading to better mutual understanding. Time spent with family watching a movie improved their mood. Persistent back pain was noted, affecting concentration, possibly due to stress. A walk during lunch provided some relief, although anxiety about work remains.",
                                                           "PatientGeneralFunctioning": "The patient is managing work and personal responsibilities effectively, though they experience occasional stress and exhaustion.",
                                                           "Interests": "The patient enjoys spending time with family and walking for relaxation.",
                                                           "SocialRelationships": "The patient had a minor conflict with their partner that was resolved positively. They also spent quality time with family, which improved their mood.",
                                                           "Work": "The patient felt overwhelmed with work tasks but managed to complete them successfully, leading to a sense of pride despite exhaustion.",
                                                           "Family": "The patient enjoyed quality time with family, which lifted their spirits.",
                                                           "PhysicalHealth": "The patient reported experiencing back pain throughout the day, which negatively affected their concentration and may be stress-related.",
                                                           "ReportedProblems": "Work-related stress and exhaustion, minor argument with partner, back pain, and ongoing anxiety about work responsibilities.",
                                                           "Other": "The patient finds that taking walks helps alleviate stress, although anxiety about work remains."
                                                       }
                                                       """;

    public static string SystemInstructionsForGeneratingQuestionForUser { get; } = """
        You are responsible for generating a question for the user that will help them create a note about their day. The question should be tailored to the given topic and tags and must be in the user's preferred language.
        
        Topics:
        - "Content": The question should encourage the user to summarize their overall day, including events and their emotional state, based on the notes they have written and their recorded HappinessLevels.
        - "PatientGeneralFunctioning": Ask about the user's general emotional state and how they handled their daily responsibilities.
        - "Interests": Prompt the user to reflect on any new interests, changes in hobbies, or things they enjoyed today.
        - "SocialRelationships": Inquire about the user's relationships, whether they formed new friendships, experienced changes in romantic relationships, or had conflicts with others.
        - "Work": Ask the user to describe any changes or challenges in their professional life.
        - "Family": Encourage reflection on family events or changes in family dynamics.
        - "PhysicalHealth": Ask about the user’s physical health, including any new health issues, improvements, or ongoing symptoms.
        - "Memories": Prompt the user to think about any memories they reflected on during the day and how they felt about them.
        - "ReportedProblems": Ask the user to detail any problems they faced today, new challenges, or solutions to ongoing issues.
        - "Other": For any other topic, ask about something significant that happened which does not fit into the other categories.
        
        Ensure that the question is open-ended and encourages the user to provide thoughtful and detailed responses. The question must be generated in the language specified by the user.
        
        **Examples:**
        
        1. **Input**:
           - Topic: "Work"
           - Language: "en"
           - Tags: ["challenges", "productivity"]
           
           **Generated Question**:
           "What challenges did you face at work today, and how did you manage to stay productive?"
        
        2. **Input**:
           - Topic: "Family"
           - Language: "pl"
           - Tags: ["family time", "conflict resolution"]
           
           **Generated Question**:
           "Jakie wydarzenia miały miejsce dziś w Twoim życiu rodzinnym? Czy doszło do jakichś konfliktów, które musiałeś/aś rozwiązać?"
        """;
}