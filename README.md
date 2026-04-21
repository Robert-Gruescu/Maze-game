# Muze Game

Un joc tip labirint realizat in Unity.

## Ce se urca pe GitHub

Acest proiect este configurat sa urce doar fisierele necesare pentru colaborare si build:

- `Assets/`
- `Packages/`
- `ProjectSettings/`

## Ce NU se urca pe GitHub

Folderele generate automat de Unity (cache, log-uri, setari locale) sunt excluse prin `.gitignore`:

- `Library/`
- `Temp/`
- `Logs/`
- `Obj/`
- `Build/`
- `Builds/`
- `UserSettings/`

## Cum publici proiectul pe GitHub

1. Deschide terminalul in folderul proiectului.
2. Ruleaza comenzile:

```bash
git init
git add .
git commit -m "Initial Unity project commit"
git branch -M main
git remote add origin https://github.com/USERNAME/REPO.git
git push -u origin main
```

## Cerinte

- Unity (aceeasi versiune folosita la dezvoltare)

## Observatie

Daca ai adaugat proiectul in Git inainte sa existe `.gitignore`, ruleaza:

```bash
git rm -r --cached Library Temp Logs UserSettings Obj Build Builds
git add .
git commit -m "Apply Unity gitignore"
```
