#include <iostream>
#include <stdexcept>
using namespace std;

struct Node {
    int data;
    Node* next;
    Node(int value) : data(value), next(nullptr) {}
    
    ~Node() {
        cout << "Remove node " << data << endl;
    }
};
class Queue {
    Node* front;
    Node* rear;
    
public:
    Queue() : front(nullptr), rear(nullptr) {}
    ~Queue() {
        while (!isEmpty()) {
            dequeue();
        }
    }
};
Queue* createQueue() {
    return new Queue();
}

bool isEmpty(Queue* q) {
    return q->front == nullptr;
}

void enqueue(Queue* q, int value) {
    Node* newNode = new Node(value);
    if (isEmpty(q)) {
        q->front = q->rear = newNode;
    } else {
        q->rear->next = newNode;
        q->rear = newNode;
    }
}

int dequeue(Queue* q) {
    if (isEmpty(q)) {
        throw out_of_range("Queue is empty");
    }
    
    Node* temp = q->front;
    int value = temp->data;
    
    q->front = q->front->next;
    if (q->front == nullptr) {
        q->rear = nullptr;
    }
    
    delete temp;
    return value;
}

void printQueue(Queue* q) {
    Node* current = q->front;
    if (isEmpty(q)) {
        cout << "Queue is empty" << endl;
        return;
    }
    
    cout << "Queue: ";
    while (current != nullptr) {
        cout << current->data;
        if (current->next != nullptr) {
            cout << " -> ";
        }
        current = current->next;
    }
    cout << endl;
}

int main() {
    Queue* q = createQueue();
    enqueue(q, 1);
    enqueue(q, 2);
    enqueue(q, 3);
    cout << dequeue(q) << endl;
    printQueue(q);
    return 0;
}
