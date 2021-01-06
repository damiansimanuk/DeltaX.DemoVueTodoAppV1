import { PaginatingTodosDto, TodoCreateDto, TodoDto, TodoDtoDataTrackerResultDto, TodoUpdateDto } from '@/dtos/TodoDto'
import axios, { axiosInstance } from '@/services/axios'
import { ref, onMounted } from 'vue'

const status = ref({ loading: false, state: 0, message: "" });
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
  return result || null;
}

const createTodo = async (todo: TodoCreateDto) => {
  const result = await request<TodoDto>("POST", `/Todo/Items`, todo)
  return result || null;
}

const deleteTodo = async (id: number) => {
  const result = await request<TodoDto>("DELETE", `/Todo/Item/${id}`)
  return result || null;
}

const getTodosSince = async (since = { since: new Date(), timeout: 60 }) => {
  const result = await axiosInstance.post<TodoDtoDataTrackerResultDto>(`/Todo/Items/GetSince`, since, { timeout: (1100 * since.timeout) })
  if (result.data?.last) {
    getTodosSince({ since: result.data?.last, timeout: 60 })
    getTodos(todos.value?.skipCount, todos.value?.maxResultCount)
  }
  else {
    getTodosSince()
  }
}

clearStatus()
getTodos(todos.value?.skipCount, todos.value?.maxResultCount);
getTodosSince()


export {
  status,
  todos,
  clearStatus,
  getTodo,
  getTodos,
  updateTodo,
  createTodo,
  deleteTodo
} 