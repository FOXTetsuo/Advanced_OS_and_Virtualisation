#include <iostream>
#include <string>

class Interpreter {
public:
    void run() {
        std::string input;
        while (true) {
            std::cout << ">> ";
            std::getline(std::cin, input);

            if (input == "exit") {
                break;
            }

            processInput(input);
        }
    }

private:
    void processInput(const std::string& input) {
        // For now, just echo the input. Later, you'll add parsing and interpreting logic here.
        std::cout << "You entered: " << input << std::endl;
    }
};

int main() {
    Interpreter interpreter;
    interpreter.run();
    return 0;
}
