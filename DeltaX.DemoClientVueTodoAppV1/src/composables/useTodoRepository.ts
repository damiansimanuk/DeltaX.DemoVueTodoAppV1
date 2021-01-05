import { PaginatingTodosDto, TodoCreateDto, TodoDto, TodoUpdateDto } from '@/dtos/TodoDto'
import axios from '@/services/axios'
import { ref, onMounted } from 'vue'

export const status = ref({ loading: false, state: 0, message: "" });

export default function useTodoRepositories() {
  const todos = ref({} as PaginatingTodosDto | null)
  const { request } = axios(status)

  const clearStatus = () => {
    status.value.loading = false;
    status.value.message = "";
    status.value.state = 0;
  }

  const getTodo = async (id: number) => {
    const result = await request<TodoDto>("GET", `/Todo/Item/${id}`)
    return result || null;
  }

  const getTodos = async (skipCount = 0, maxResultCount = 10) => {
    const result = await request<PaginatingTodosDto>("GET", `/Todo/Items?skipCount=${skipCount}&maxResultCount=${maxResultCount}`)
    todos.value = result || null;
    return result || null;
  }

  const updateTodo = async (todo: TodoUpdateDto) => {
    const result = await request<TodoDto>("PUT", `/Todo/Item/${todo.id}`, todo)
    getTodos(todos.value?.skipCount, todos.value?.maxResultCount)
    return result || null;
  }

  const createTodo = async (todo: TodoCreateDto) => {
    const result = await request<TodoDto>("POST", `/Todo/Items`, todo)
    getTodos(todos.value?.skipCount, todos.value?.maxResultCount)
    return result || null;
  }

  const deleteTodo = async (id: number) => {
    const result = await request<TodoDto>("DELETE", `/Todo/Item/${id}`)
    getTodos(todos.value?.skipCount, todos.value?.maxResultCount)
    return result || null;
  }

  onMounted(() => {
    clearStatus()
    getTodos()
  })

  return {
    status,
    todos,
    clearStatus,
    getTodo,
    getTodos,
    updateTodo,
    createTodo,
    deleteTodo
  }
}