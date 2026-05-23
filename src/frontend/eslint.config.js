import js from '@eslint/js'
import globals from 'globals'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import react from 'eslint-plugin-react'
import tseslint from 'typescript-eslint'
import prettier from 'eslint-plugin-prettier'
import prettierConfig from 'eslint-config-prettier'

export default tseslint.config(
    // Игнорируемые директории
    {
        ignores: ['dist', 'node_modules', 'coverage', '*.config.js', '*.config.ts'],
    },

    // Базовые правила JS
    js.configs.recommended,

    // TypeScript — строгий режим
    ...tseslint.configs.strictTypeChecked,
    ...tseslint.configs.stylisticTypeChecked,

    {
        files: ['**/*.{ts,tsx}'],

        languageOptions: {
            ecmaVersion: 2024,
            globals: globals.browser,
            parserOptions: {
                project: ['./tsconfig.app.json'],
                tsconfigRootDir: import.meta.dirname,
                ecmaFeatures: {
                    jsx: true,
                },
            },
        },

        plugins: {
            react,
            'react-hooks': reactHooks,
            'react-refresh': reactRefresh,
            prettier,
        },

        settings: {
            react: {
                version: 'detect', // автоматически определяет версию React
            },
        },

        rules: {
            // ── Prettier ──────────────────────────────────────────
            'prettier/prettier': 'error',

            // ── React ─────────────────────────────────────────────
            ...react.configs.recommended.rules,
            ...react.configs['jsx-runtime'].rules, // отключает требование импорта React (React 17+)
            ...reactHooks.configs.recommended.rules,
            'react-refresh/only-export-components': ['warn', { allowConstantExport: true }],
            'react/prop-types': 'off',             // не нужен при TypeScript
            'react/display-name': 'warn',

            // ── TypeScript ─────────────────────────────────────────
            '@typescript-eslint/no-unused-vars': ['error', {
                argsIgnorePattern: '^_',             // переменные с _ игнорируются
                varsIgnorePattern: '^_',
            }],
            '@typescript-eslint/consistent-type-imports': ['error', {
                prefer: 'type-imports',              // import type { Foo } вместо import { Foo }
                fixStyle: 'inline-type-imports',
            }],
            '@typescript-eslint/no-explicit-any': 'warn',
            '@typescript-eslint/no-floating-promises': 'error',
            '@typescript-eslint/no-misused-promises': 'error',
            '@typescript-eslint/await-thenable': 'error',
            '@typescript-eslint/no-unnecessary-condition': 'warn',
            '@typescript-eslint/prefer-nullish-coalescing': 'error',
            '@typescript-eslint/prefer-optional-chain': 'error',

            // ── Общие ─────────────────────────────────────────────
            'no-console': ['warn', { allow: ['warn', 'error'] }],
            'prefer-const': 'error',
            'no-var': 'error',
            eqeqeq: ['error', 'always'],
        },
    },

    // Отключаем конфликтующие с Prettier правила (должен быть последним)
    prettierConfig,
)