#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <Windows.h>

using namespace std;

class Book {
private:
    string title;
    string author;

public:
    string getTitle() const { return title; }
    void setTitle(const string& t) { title = t; }
    
    string getAuthor() const { return author; }
    void setAuthor(const string& a) { author = a; }

    Book(const string& t, const string& a) : title(t), author(a) {}
    
    string toString() const {
        return "'" + title + "' by " + author;
    }
};
typedef void (*NotificationHandler)(const string& message);
class Library {
private:
    vector<Book*> books;
    vector<NotificationHandler> handlers;
    NotificationHandler lambdaHandler;

public:
    void addHandler(NotificationHandler handler) {
        handlers.push_back(handler);
    }    
    void setLambdaHandler(NotificationHandler handler) {
        lambdaHandler = handler;
    }
    
    void notifyAll(const string& message) {
        for (auto handler : handlers) {
            if (handler) handler(message);
        }
        if (lambdaHandler) {
            lambdaHandler(message);
        }
    }
    
    void addBook(const string& title, const string& author) {
        Book* book = new Book(title, author);
        books.push_back(book);
        notifyAll("Новая книга добавлена: " + book->toString());
    }
    
    bool takeBook(const string& title) {
        for (auto book : books) {
            if (book->getTitle() == title) {
                notifyAll("Книга взята: " + book->toString());
                return true;
            }
        }
        return false;
    }
    
    bool returnBook(const string& title) {
        for (auto book : books) {
            if (book->getTitle() == title) {
                notifyAll("Книга возвращена: " + book->toString());
                return true;
            }
        }
        return false;
    }
    
    void showBooks() {
        cout << "Книги в библиотеке\n";
        for (auto book : books) {
            cout << "- " << book->toString() << endl;
        }
    }
    
    ~Library() {
        for (auto book : books) {
            delete book;
        }
    }
};


void consoleNotify(const string& message) {
    cout << "[КОНСОЛЬ] " << message << endl;
}

class FileLogger {
private:
    ofstream logFile;
    
public:
    FileLogger(const string& filename) {
        logFile.open(filename, ios::app);
    }
    
    ~FileLogger() {
        if (logFile.is_open()) {
            logFile.close();
        }
    }
    
    void log(const string& message) {
        if (logFile.is_open()) {
            logFile << message << endl;
        }
    }
};

FileLogger* fileLogger = nullptr;
void fileNotify(const string& message) {
    if (fileLogger) {
        fileLogger->log(message);
    }
}
void popupNotify(const string& message) {
    MessageBoxA(NULL, message.c_str(), "Библиотека", MB_OK | MB_ICONINFORMATION);
}

int main() {
    Library library;
    fileLogger = new FileLogger("library.log");
    library.addHandler(consoleNotify);
    library.addHandler(fileNotify);
    library.addHandler(popupNotify);
    auto lambdaNotify = [](const string& msg) {
        cout << msg << endl;
    };
    library.setLambdaHandler(lambdaNotify);
    library.addBook("88", "Михаил");
    library.addBook("99", "Джордж");
    library.addBook("ст д", "Роберт");  
    library.showBooks();
    library.takeBook("1984");
    library.returnBook("1984");
    library.takeBook("Чистый код");   
    library.showBooks();   
    return 0;
}
