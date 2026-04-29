# Bachelorarbeit: Spielsimulation mit Graphen

Willkommen in diesem Repository, das den Quellcode für meine Bachelorarbeit enthält. Ziel der Arbeit ist es, die Anwendung von Graphenstrukturen zur Modellierung, Simulation und Analyse von Spielen zu erforschen. Das Projekt bietet eine Grundlage für die Untersuchung von Spielstrategien und deren Optimierung.

## Überblick

Das Projekt verwendet gerichtete Graphen, um Spielverläufe zu modellieren. Es umfasst:
- Die Implementierung eines Graphen, der die Struktur des Spiels abbildet.
- Einen Spielbaum, der alle möglichen Spielverläufe speichert.
- Mechanismen zur Analyse und Speicherung von Spielzuständen.

Die Arbeit zeigt, wie Graphenstrukturen genutzt werden können, um komplexe Spielsysteme effizient zu modellieren und zu analysieren.

## Verwendete Technologien und Bibliotheken

- **.NET 8**: Die neueste Version der .NET-Plattform für moderne und leistungsstarke Anwendungen.
- **QuikGraph**: Eine vielseitige .NET-Bibliothek zur Arbeit mit Graphen, die Funktionen wie Graphenmanipulation und -analyse bietet.

## Hauptklassen und deren Funktionen

- **FiniteDirectedGraph**: 
  - Erstellt und verwaltet gerichtete Graphen.
  - Dient als Grundlage für die Modellierung des Spiels.
  - Unterstützt Operationen wie das Hinzufügen von Knoten und Kanten.

- **GameTree**: 
  - Speichert den gesamten möglichen Spielverlauf in Form eines Graphen.
  - Ermöglicht die Analyse von Spielstrategien durch Traversierung des Baums.

- **Positions**: 
  - Speichert alle relevanten Daten zum aktuellen Zustand des Spiels.
  - Bietet Mechanismen zur Aktualisierung und Abfrage von Spielzuständen.

## Zielsetzung

Das Hauptziel des Projekts ist es, die Vorteile von Graphenstrukturen für die Modellierung und Analyse von Spielen aufzuzeigen. Es soll demonstriert werden, wie diese Strukturen genutzt werden können, um:
- Komplexe Spielsysteme effizient zu simulieren.
- Strategien zu analysieren und zu optimieren.
- Ein besseres Verständnis für die Dynamik von Spielen zu gewinnen.
