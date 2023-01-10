import type { PageLoad } from './$types';

export const load = ( async ({ params, fetch }) => {
    const response = await fetch('/api/Pizza');
    const pizzas = await response.json();

    return {
        post: {
            title: "Title",
            content: "Content",
            pizzas: pizzas
        }
    }
}) satisfies PageLoad;