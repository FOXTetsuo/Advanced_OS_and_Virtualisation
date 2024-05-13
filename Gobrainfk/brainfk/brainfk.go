package main

import "os"

// the goal here is to write an interpreter for brainfuck.
const examplebrainfuck = ">++++++++[<+++++++++>-]<.>++++[<+++++++>-]<+.+++++++..+++.>>++++++[<+++++++>-]<+\n+.------------.>++++++[<+++++++++>-]<+.<.+++.------.--------.>>>++++[<++++++++>-\n]<+."
const memsize = 30000

var memory [memsize]int

// Current instruction pointer
var ipointer = 0

// Memory pointer
var mpointer = 0

// Address stack
var astack []int

// Brainfuck program code
var program string
var input string
var output string

func main() {

	program = examplebrainfuck

	println(interpret())
}

func interpret() string {
	end := false

	for !end {
		switch program[ipointer] {
		case '>':
			if mpointer == len(memory)-1 {
				println("OUT OF MEMORY")
				os.Exit(2)
			}
			mpointer++
		case '<':
			if mpointer > 0 {
				mpointer--
			}
		case '+':
			memory[mpointer]++
		case '-':
			memory[mpointer]--
		case '.':
			sendOutput(memory[mpointer])
		case ',':
			memory[mpointer] = getInput()
		case '[':
			if memory[mpointer] != 0 {
				astack = append(astack, ipointer)
			} else {
				count := 0
				for {
					ipointer++
					if ipointer >= len(program) {
						break
					}
					if program[ipointer] == '[' {
						count++
					} else if program[ipointer] == ']' {
						if count != 0 {
							count--
						} else {
							break
						}
					}
				}
			}
		case ']':
			// Pointer is automatically incremented every iteration,
			// therefore we must decrement to get the correct value
			if len(astack) > 0 && memory[mpointer] != 0 {
				ipointer = astack[len(astack)-1]
			} else {
				return "Error: Unmatched ']'"
			}
			astack = astack[:len(astack)-1] // Pop from stack
		case 0: // We have reached the end of the program
			end = true
		default: // We ignore any character that are not part of regular Brainfuck syntax
		}
		ipointer++
	}
	return output
}

func resetState() {
	// Clear memory, reset pointers to zero.
	for i := range memory {
		memory[i] = 0
	}
	ipointer = 0
	mpointer = 0
	output = ""
	input = ""
	program = ""
	astack = nil
}

func getInput() int {
	var inputvalue = 0

	if input != "" {
		inputvalue = int(input[0])
		input = input[1:]
	}

	return inputvalue
}

func sendOutput(value int) {
	output += string(rune(value))
}
