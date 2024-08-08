const fs = require("fs");
const readlineSync = require("readline-sync");

function showQuestion(questionDict) {
  console.log(`\n ${questionDict.q}`);

  if (questionDict.type === "true_false") {
    showTrueFalseOptions();
  } else if (questionDict.type === "fill_in_the_blank") {
    showFillInTheBlankPrompt();
  }
}

function showTrueFalseOptions() {
  console.log(" <a> True");
  console.log(" <b> False");
}

function showFillInTheBlankPrompt() {
  console.log(" Fill in the blank: ");
}

function getUserAnswer(questionDict) {
  let answer;

  do {
    answer = readlineSync.question("\n Enter your answer: ").toLowerCase();

    // Validate user input based on the question type
    if (questionDict.type === "true_false") {
      if (!["true", "false"].includes(answer)) {
        console.log("\nInvalid option. Please enter 'true' or 'false'.");
        continue; 
      }
    } else if (questionDict.type === "fill_in_the_blank") {
      if (answer !== questionDict.an.toLowerCase()) {
        console.log(
          "\nIncorrect answer. The correct answer is: " + questionDict.an
        );
        break; 
      }
    }
  } while (
    (questionDict.type === "true_false" &&
      !["true", "false"].includes(answer)) ||
    (questionDict.type === "fill_in_the_blank" &&
      answer !== questionDict.an.toLowerCase())
  );

  return answer;
}

// Shuffle function
function shuffle(array) {
  for (let i = array.length - 1; i > 0; i--) {
    const j = Math.floor(Math.random() * (i + 1));
    [array[i], array[j]] = [array[j], array[i]];
  }
}

// Load questions and shuffle them
let questions;
const filePath = "./question.json"; 
////TO RUN THIS CODE IN TERMINAL WRITE: node javascript

function loadAndShuffleQuestions(filePath, difficulty) {
  try {
    if (!fs.existsSync(filePath)) {
      throw new Error("File not found");
    }
    questions = JSON.parse(fs.readFileSync(filePath, "utf-8"));
    questions = questions.filter((q) => q.difficulty === difficulty);
    shuffle(questions);
  } catch (error) {
    console.error(`Error loading questions: ${error.message}`);
    process.exit(1); 
  }
}
const difficultyOptions = ["starter", "advanced"];
const selectedDifficultyIndex = readlineSync.keyInSelect(
  difficultyOptions,
  "Choose difficulty:"
);

if (selectedDifficultyIndex === -1) {
  console.log("Exiting the game.");
  process.exit(0);
}
const selectedDifficulty = difficultyOptions[selectedDifficultyIndex];
loadAndShuffleQuestions(filePath, selectedDifficulty);

const numberOfQuestionsToAsk = 5;
let rightAnswer = 0;
let currentQuestionIndex = 0;

function askNextQuestion() {
  if (
    currentQuestionIndex < numberOfQuestionsToAsk &&
    currentQuestionIndex < questions.length
  ) {
    const questionDict = questions[currentQuestionIndex];
    showQuestion(questionDict);

    const userAnswer = getUserAnswer(questionDict);

    if (userAnswer === questionDict.an.toLowerCase()) {
      rightAnswer++;
      console.log("\nCorrect Answer!");
    } else {
      console.log("\nYour answer is wrong");
      console.log(`\nCorrect answer is ${questionDict.an}`);
    }

    currentQuestionIndex++;
    askNextQuestion(); 
  } else {
    generateReport();
  }
}
askNextQuestion();
function generateReport() {
  console.log("\n!!!Game Over");
  console.log(
    `\nYou made ${rightAnswer} right out of ${numberOfQuestionsToAsk} answers. Your score is ${rightAnswer}`
  );
}
