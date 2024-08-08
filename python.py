import json
import os
import random
import time

def show_question(question_dict):
    if 'q_type' in question_dict:
        print("\n {}) {}".format(question_dict['q_type'].capitalize(), question_dict['q']))
    else:
        print("\n {}".format(question_dict['q']))

    if question_dict.get('q_type') == 'true_false':
        show_true_false_options()
    elif question_dict.get('q_type') == 'fill_in_the_blank':
        show_fill_in_the_blank_prompt()

def show_true_false_options():
    print(" <a> True")
    print(" <b> False")

def show_fill_in_the_blank_prompt():
    print(" Fill in the blank: ")

# You can set the time for each question here, default is 3 sec but you can override this, work in seconds
default_time_limit = 5

def check_answer(question_dict, user_answer, elapsed_time, time_limit):
    correct_answer = question_dict['an'].lower()  

    if user_answer == correct_answer and elapsed_time <= time_limit:
        print("\nCorrect Answer!")
        return True
    else:
        if elapsed_time > time_limit:
            print("\nTime's up! You did not give the answer on time.")
        else:
            print("\nYour answer is wrong")
            print("Correct answer is {correct_answer}".format(correct_answer=correct_answer))
        return False


difficulty_level = input("Choose difficulty level (starter/advanced): ").lower()

#TO RUN THIS CODE IN TERMINAL WRITE: python 200302021_Alem_homeowork_2.py
with open("question.json", "r", encoding='utf-8') as qa:
    question_set = qa.read()
    questions_list = json.loads(question_set)
    right_answer = 0
    i = 0

# Randomized questions every time 
random.shuffle(questions_list)

# Choose the level of difficulty
filtered_questions = [q for q in questions_list if q['difficulty'] == difficulty_level]
filtered_questions = filtered_questions[:5]  
total_quiz_time_limit = len(filtered_questions) * default_time_limit

# Iterate through each question on the list
while i < len(filtered_questions):
    question_dict = filtered_questions[i]
    
    print("\nTime limit for this question: {} seconds".format(default_time_limit))
    
    show_question(question_dict)

    start_time = time.time()
    user_answer = input("\n Enter your answer: ").lower()
    elapsed_time = time.time() - start_time

    is_correct = check_answer(question_dict, user_answer, elapsed_time, default_time_limit)

    if is_correct:
        right_answer += 1

    print("Current Score: {}/{}".format(right_answer, i + 1))

    i += 1

print("\n!!!Game Over")
print("You made {} right out of {} answers. Your final score is {} ".format(right_answer, len(filtered_questions), str(right_answer)))
print("Total quiz time: {} seconds".format(total_quiz_time_limit))

