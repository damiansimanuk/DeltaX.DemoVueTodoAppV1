<template>
  <div class="container mx-auto py-3"> 
    <div class="new-todo-form flex column my-2">
      <input
        class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        type="text"
        v-model="newTodo"
        placeholder="Create new todo" 
        @keyup.enter="addTodo">
      <button 
        class="bg-white hover:bg-blue-100 text-gray-800 font-semibold ml-2 py-2 px-2 ripple border border-gray-400 rounded shadow" 
        @click="addTodo" > 
        <svg class="w-5 h-5 text-red fill-current" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
        </svg>
      </button> 
    </div>
    <ul v-if="todos" > 
      <div v-for="t in todos.items" :key="t.id"   >
        <TodoItem :todo="t" @update="onUpdate" @remove="onRemove" />
      </div>
    </ul>
  </div>
</template>

<script lang="ts">
import { ref, reactive } from 'vue'
import useTodoRepositories from "@/composables/useTodoRepository";
import TodoItem from "@/components/TodoItem.vue"

export default {
  components:{
    TodoItem
  },

  setup () {
    const state = reactive({
      count: 0,
    })
    const newTodo = ref("")
    const { todos, getTodos, status, updateTodo, createTodo, deleteTodo } = useTodoRepositories()
 
    const addTodo = async function () { 
      await createTodo({description: newTodo.value, completed: false})
      newTodo.value = "";
      getTodos(todos.value?.skipCount, todos.value?.maxResultCount);
    }

    const onUpdate =  (id: number, description: string, completed: boolean)=> { 
      console.log("onCompleted", id, description, completed);
      const todo = todos.value?.items.find(t => t.id == id)
      if(todo)
      {
        updateTodo({id : id, description : description, completed : completed })
      }
    }
    
    const onRemove =  (id: number)=> { 
      console.log("onRemove", id);
      const todo = todos.value?.items.find(t => t.id == id)
      if(todo)
      {
        deleteTodo(id)
      }
    }

    return {
      newTodo,
      addTodo,
      todos, 
      state,
      status,  
      onUpdate,
      onRemove
    }
  }
}
</script>